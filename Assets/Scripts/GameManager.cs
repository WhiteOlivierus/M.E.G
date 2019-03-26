using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Scenario[] allScenarios = new Scenario[0];
    [Space] [SerializeField] private TextMesh resultText = new TextMesh();
    [SerializeField] private TextMesh goalText = new TextMesh();
    [SerializeField] private SpriteRenderer resultSprite = new SpriteRenderer();
    [Space] [SerializeField] private SliderComponent[] allSliders = new SliderComponent[0];
    [Space] [SerializeField] private TextMesh turns = new TextMesh();
    [SerializeField] private int maxTurns = 0;
    [Space] public GameObject chargePort;
    [Space] [SerializeField] private EarthController earthController;

    private int turnsLeft;
    private Scenario currentScenario;
    private DragableComponent connectedBattery;

    void Awake()
    {
        allScenarios = Resources.LoadAll<Scenario>("Scenarios");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        for (int i = 0; i < allSliders.Length; i++)
        {
            ScenarioType s = (ScenarioType)i;
            allSliders[i].sliderName.text = s.ToString();
        }
    }

    public void CheckGameState()
    {
        if (connectedBattery == null) { return; }

        SetTurns();

        if (CheckIfOutOfTurns())
        {
            ReleaseBattery();
            ShowResult(-1);
            return;
        }

        ShowResult(FindClosestScenarioToSliders());
    }

    private void ShowResult(int index)
    {
        if (index >= 0)
        {
            string scenarioResult = allScenarios[index].scenarioName;
            resultSprite.sprite = allScenarios[index].scenario;
            earthController.SetAllMaterials(allScenarios[index].earthValues);
            SetText(resultText, scenarioResult);
        }
        else
        {
            SetText(goalText, "Replace batery for next goal");
            SetText(resultText, "None");
            resultSprite.sprite = null;
        }
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

        foreach (SliderComponent slider in allSliders)
        {
            slider.SetPrecisionMonitor(-3);
        }

        connectedBattery.Release();
        connectedBattery = null;
    }

    public bool InitGame(DragableComponent cb)
    {
        if (connectedBattery != null) { return false; }

        connectedBattery = cb;
        turnsLeft = maxTurns;
        turns.text = turnsLeft.ToString();
        currentScenario = allScenarios[Random.Range(0, allScenarios.Length)];
        SetText(goalText, currentScenario.scenarioName);
        return true;
    }
}
