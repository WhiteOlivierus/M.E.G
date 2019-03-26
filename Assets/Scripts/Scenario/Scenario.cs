using UnityEngine;

[CreateAssetMenu(fileName = "Scenario", menuName = "Scenario", order = 0)]
public class Scenario : ScriptableObject
{
    public Slider[] sliders = new Slider[6];

    public string scenarioName;

    public Sprite scenario;

    public float[] earthValues = new float[5];

    public void Awake()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].type = (ScenarioType)i;
        }
    }
}

public enum ScenarioType
{
    economy = 0,
    policies = 1,
    rescources = 2,
    population = 3,
    technologie = 4,
    lifestyle = 5
}