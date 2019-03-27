using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
        SetText(resultText, "None");
        resultSprite.sprite = null;
    }

    private void ShowRight(int index)
    {
        string scenarioResult = allScenarios[index].scenarioName;
        resultSprite.sprite = allScenarios[index].scenario;
        earthController.SetAllMaterials(allScenarios[index].earthValues);
        SetText(resultText, scenarioResult);
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
        for (int i = 0; i < screenMaterials.Length; i++)
        {
            screenMaterials[i] = screenRenderers[i].material;
            screenRenderers[i].material = wrongMaterial;
        }

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < screenMaterials.Length; i++)
        {
            screenRenderers[i].material = screenMaterials[i];
        }
    }

}
