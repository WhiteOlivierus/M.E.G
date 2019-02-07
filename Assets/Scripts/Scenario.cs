using UnityEngine;

[CreateAssetMenu(fileName = "Scenario", menuName = "M.E.G/Scenario", order = 0)]
public class Scenario : ScriptableObject
{
    public Vector2 economy;
    public Vector2 climate;
    public Vector2 policy;
    public Vector2 rescources;

    public string scenarioName;

    public Sprite scenario;
}
