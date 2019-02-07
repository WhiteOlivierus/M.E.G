using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Scenario[] allScenarios;
    public Text result;
    public Text pointsToSpend;
    public int maxPointsToSpend;
    public Text goalText;

    public Slider economySlider;
    public Slider climateSlider;
    public Slider policySlider;
    public Slider rescourcesSlider;

    public Button generateBtn;

    private int[] snapShot = new int[4];
    private int[] lastSnapShot = new int[4];

    private void Start()
    {
        CreateSnapShot();
        for (int i = 0; i < snapShot.Length; i++)
        {
            lastSnapShot[i] = snapShot[i];
        }
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

        CreateSnapShot();
    }

    public void ConstrainPoints(Slider s)
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
            generateBtn.interactable = false;
        }
        else
        {
            generateBtn.interactable = true;
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
    }

    private void RevertSnapShot()
    {
        economySlider.value = snapShot[0];
        climateSlider.value = snapShot[1];
        policySlider.value = snapShot[2];
        rescourcesSlider.value = snapShot[3];
    }
}
