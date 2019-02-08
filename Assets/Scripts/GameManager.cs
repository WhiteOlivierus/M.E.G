using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Scenario[] allScenarios;
    [Space]
    public Text result;
    public Image resultImage;
    public GameObject resultMonitor;
    public Material resultMaterial;
    [Space]
    public Text pointsToSpend;
    public int maxPointsToSpend;
    public Text goalText;
    [Space]
    public SliderComponent economySlider;
    public SliderComponent climateSlider;
    public SliderComponent policySlider;
    public SliderComponent rescourcesSlider;
    [Space]
    public BoxCollider generateBtn;
    [Space]
    public Text turns;
    public int maxTurns;

    private int[] snapShot = new int[4];
    private int[] lastSnapShot = new int[4];

    private void Start()
    {
        CreateSnapShot();
        SetGoal();
        pointsToSpend.text = maxPointsToSpend.ToString();
    }

    public void CheckScenario()
    {
        int index = 0;
        int lastScore = 0;
        for (int i = 0; i < allScenarios.Length; i++)
        {
            int score = 0;
            if (allScenarios[i].economy.x > economySlider.value || allScenarios[i].economy.y < economySlider.value) score++;
            if (allScenarios[i].climate.x > economySlider.value || allScenarios[i].climate.y < economySlider.value) score++;
            if (allScenarios[i].policy.x > economySlider.value || allScenarios[i].policy.y < economySlider.value) score++;
            if (allScenarios[i].rescources.x > economySlider.value || allScenarios[i].rescources.y < economySlider.value) score++;

            if (score >= lastScore)
            {
                index = i;
                lastScore = score;
            }
        }

        result.text = allScenarios[index].scenarioName;
        resultImage.sprite = allScenarios[index].scenario;
        StartCoroutine("EndState");

        ResetSliders();
        CreateSnapShot();
        turns.text = (int.Parse(turns.text) + 1).ToString();
        pointsToSpend.text = maxPointsToSpend.ToString();
    }

    public void ConstrainPoints(SliderComponent s)
    {
        //current slider in use
        int index = int.Parse(s.name);
        //current point total
        int points = int.Parse(pointsToSpend.text);
        //result for this slider
        int result = 0;

        //calculate how much points have to be removed/added
        int direction = lastSnapShot[index] - (int)s.value;
        result = direction;

        //calculate the side of the orgin you are
        int side = (int)s.value - snapShot[index];

        if (side == 0)
        {
            //if you are back at base pos at 1 to points
            if (direction < 0)
                direction *= -1;
            points += direction;
        }
        else if (side > 0)
        {
            //Rightside of the slider
            print("Right side");
            if (direction < 0)
            {
                direction *= -1;
                points -= direction;
            }
            else if (direction > 0)
            { points += direction; }
        }
        else if (side < 0)
        {
            //left side of the slider
            print("Left side");
            if (direction < 0)
            {
                direction *= -1;
                points += direction;
            }
            else if (direction > 0)
            { points -= direction; }
        }

        if (points < 0)
        {
            generateBtn.enabled = false;
        }
        else
        {
            generateBtn.enabled = true;
        }

        //update the live slider snapshot to lock it
        lastSnapShot[index] -= result;
        pointsToSpend.text = points.ToString();
    }

    public void SetGoal()
    {
        goalText.text = allScenarios[Random.Range(0, allScenarios.Length)].scenarioName;
        pointsToSpend.text = maxPointsToSpend.ToString();
    }

    private void CreateSnapShot()
    {
        snapShot[0] = (int)economySlider.value;
        snapShot[1] = (int)climateSlider.value;
        snapShot[2] = (int)policySlider.value;
        snapShot[3] = (int)rescourcesSlider.value;

        for (int i = 0; i < snapShot.Length; i++)
        {
            lastSnapShot[i] = snapShot[i];
        }
    }

    private void ResetSliders()
    {
        economySlider.position = 1;
        climateSlider.position = 1;
        policySlider.position = 1;
        rescourcesSlider.position = 1;
    }

    private void Update()
    {
        if (int.Parse(turns.text) > maxTurns)
        {
            turns.text = "0";
            ResetSliders();
            CreateSnapShot();
            SetGoal();
            generateBtn.enabled = true;
        }
    }


    IEnumerator EndState()
    {
        Material t = resultMonitor.GetComponent<Renderer>().material;
        resultMonitor.GetComponent<Renderer>().material = resultMaterial;
        yield return new WaitForSeconds(5f);
        resultMonitor.GetComponent<Renderer>().material = t;
    }
}
