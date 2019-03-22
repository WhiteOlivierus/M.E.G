using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Scenario[] allScenarios = new Scenario[0];
    [Space] [SerializeField] private TextMesh resultText = new TextMesh();
    [SerializeField] private TextMesh goalText = new TextMesh();
    [Space] [SerializeField] private SliderComponent[] allSliders = new SliderComponent[0];
    [Space] [SerializeField] private TextMesh turns = new TextMesh();
    [SerializeField] private int maxTurns = 0;
    [Space] public GameObject chargePort;

    private int turnsLeft;
    private Scenario currentScenario;
    private DragableComponent connectedBattery;

    void Start()
    {
        for (int i = 0; i < allSliders.Length; i++)
        {
            ScenarioType s = (ScenarioType)i;
            allSliders[i].sliderName.text = s.ToString();
        }
    }

    public void CheckGameState()
    {
        SetTurns();

        if (CheckIfOutOfTurns())
        {
            ReleaseBattery();
            SetText(goalText, "Replace batery for next goal");
            SetText(resultText, "None");
            return;
        }

        string scenarioResult = allScenarios[FindClosestScenarioToSliders()].scenarioName;
        SetText(resultText, scenarioResult);
    }

    private void SetTurns()
    {
        turnsLeft -= 1;
        turns.text = turnsLeft.ToString();
        Renderer r = connectedBattery.transform.GetChild(0).GetComponent<Renderer>();
        r.material.SetColor("_EmissionColor", r.material.GetColor("_EmissionColor") * .85f);
        DynamicGI.SetEmissive(r, r.material.GetColor("_EmissionColor") * .85f);
    }

    private void SetText(TextMesh output, string text)
    {
        output.text = text;
    }

    private int FindClosestScenarioToSliders()
    {
        int index = 0;
        int lastScore = 0;

        for (int i = 0; i < allScenarios.Length; i++)
        {
            // Scenario currentScenario = allScenarios[i];
            int score = 0;

            for (int j = 0; j < allSliders.Length; j++)
            {
                Slider currentSlider = currentScenario.sliders[j];
                float x = allSliders[j].value;
                if (allSliders[j].value >= currentSlider.between.y)
                {
                    score += (int)Mathf.Abs(allSliders[j].value - currentSlider.between.y);
                    allSliders[j].SetPrecisionMonitor(1);
                }
                else if (allSliders[j].value <= currentSlider.between.x)
                {
                    score += (int)Mathf.Abs(allSliders[j].value - currentSlider.between.x);
                    allSliders[j].SetPrecisionMonitor(-1);
                }
                else
                {
                    allSliders[j].SetPrecisionMonitor(0);
                }
            }

            if (score <= lastScore)
            {
                index = i;
                lastScore = score;

                if (lastScore == 0)
                {
                    Debug.Log("Winner winner chicken diner");
                }
            }
        }

        return index;
    }

    private bool CheckIfOutOfTurns()
    {
        return turnsLeft == 0;
    }

    private void ReleaseBattery()
    {
        if (connectedBattery == null) { return; }

        connectedBattery.ReleaseBatery();
    }

    public void InitGame(DragableComponent cb)
    {
        connectedBattery = cb;
        turnsLeft = maxTurns;
        turns.text = turnsLeft.ToString();
        currentScenario = allScenarios[Random.Range(0, allScenarios.Length)];
        SetText(goalText, currentScenario.scenarioName);
    }
}
