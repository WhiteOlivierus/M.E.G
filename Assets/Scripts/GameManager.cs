using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Scenario[] allScenarios = new Scenario[0];
    [Space] [SerializeField] private TextMeshPro resultText = new TextMeshPro();
    [SerializeField] private TextMeshPro goalText = new TextMeshPro();
    [SerializeField] private SpriteRenderer resultSprite = new SpriteRenderer();
    [Space] [SerializeField] private SliderComponent[] allSliders = new SliderComponent[0];
    [Space] [SerializeField] private TextMesh turns = new TextMesh();
    [SerializeField] private int maxTurns = 0;
    [Space] public GameObject chargePort;
    [Space] [SerializeField] private GameObject earthController;
    [Space] [SerializeField] private Material wrongMaterial;

    private int turnsLeft;
    private Scenario currentScenario;
    private DragableComponent connectedBattery;
    private TextMesh[] precisionMonitors;
    private SpriteRenderer[] iconMonitors;
    private Sprite[] icons;
    private TextMesh[] lastValues;
    private Renderer[] screenRenderers;
    private Material[] screenMaterials;
    private bool notShowingWrong = true;

    void Awake()
    {
        precisionMonitors = GameObject.FindWithTag("precision").GetComponentsInChildren<TextMesh>();
        iconMonitors = GameObject.FindWithTag("icon").GetComponentsInChildren<SpriteRenderer>();
        lastValues = GameObject.FindWithTag("lastValue").GetComponentsInChildren<TextMesh>();
        screenRenderers = GameObject.FindWithTag("screens").GetComponentsInChildren<Renderer>();
        screenMaterials = new Material[screenRenderers.Length];
        allScenarios = Resources.LoadAll<Scenario>("Scenarios");
        icons = Resources.LoadAll<Sprite>("Icons");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        ShowEmpty();

        for (int i = 0; i < allSliders.Length; i++)
        {
            allSliders[i].sliderName.Add(iconMonitors[i]);
            allSliders[i].precisionMonitor = precisionMonitors[i];
            ScenarioType scenarioT = (ScenarioType)i;

            foreach (SpriteRenderer s in allSliders[i].sliderName)
            {
                s.sprite = icons[i];
            }
        }
    }

    public void CheckGameState()
    {
        if (!notShowingWrong) { return; }
        if (connectedBattery == null) { return; }

        SetTurns();

        if (CheckIfOutOfTurns())
        {
            ReleaseBattery();
            ShowEmpty();
            return;
        }
        SetLastValues();

        ShowResult(FindClosestScenarioToSliders());
    }

    private void SetLastValues()
    {
        for (int i = 0; i < allSliders.Length; i++)
        {
            lastValues[i].text = allSliders[i].value.ToString();
        }
    }

    private void ShowResult(int index)
    {
        if (index < 0)
        {
            ShowWrong();
        }
        else
        {
            ShowRight(index);
        }
    }

    private void ShowEmpty()
    {
        SetText(goalText, "Replace batery for next goal");
        SetText(resultText, "Replace batery for next goal");
        resultSprite.sprite = null;
    }

    private void ShowRight(int index)
    {
        resultSprite.sprite = currentScenario.scenario;
        earthController.GetComponent<EarthController>().SetAllMaterials(currentScenario.earthValues);
        SetText(resultText, currentScenario.scenarioName);
        ReleaseBattery();
    }

    private void ShowWrong()
    {
        StartCoroutine("Wrong");
    }

    private void SetTurns()
    {
        turnsLeft -= 1;
        turns.text = turnsLeft.ToString();
        Renderer r = connectedBattery.transform.GetChild(0).GetComponent<Renderer>();
        r.material.SetColor("_EmissionColor", r.material.GetColor("_EmissionColor") * .85f);
        DynamicGI.SetEmissive(r, r.material.GetColor("_EmissionColor") * .85f);
    }

    private void SetText(TextMeshPro output, string text)
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
                float sliderValue = allSliders[j].value;
                if (sliderValue >= currentSlider.between.y)
                {
                    score += (int)Mathf.Abs(sliderValue - currentSlider.between.y);
                    allSliders[j].SetPrecisionMonitor(1);
                }
                else if (sliderValue <= currentSlider.between.x)
                {
                    score += (int)Mathf.Abs(sliderValue - currentSlider.between.x);
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
                    return index;
                }
            }
        }

        return -1;
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
        currentScenario = allScenarios[UnityEngine.Random.Range(0, allScenarios.Length)];
        SetText(goalText, currentScenario.scenarioName);
        resultSprite.sprite = null;
        return true;
    }

    private IEnumerator Wrong()
    {
        notShowingWrong = false;
        SetScreensToWrong();
        yield return new WaitForSeconds(3f);
        SetScreensBack();
        notShowingWrong = true;
    }

    private void SetScreensBack()
    {
        for (int i = 0; i < screenMaterials.Length; i++)
        {
            screenRenderers[i].material = screenMaterials[i];
        }
    }

    private void SetScreensToWrong()
    {
        for (int i = 0; i < screenMaterials.Length; i++)
        {
            screenMaterials[i] = screenRenderers[i].material;
            screenRenderers[i].material = wrongMaterial;
        }
    }
}
