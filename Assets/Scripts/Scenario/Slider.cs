using UnityEngine;
[System.Serializable]
public class Slider
{
    public ScenarioType type;
    public Vector2 between;

    public Slider(ScenarioType t)
    {
        this.type = t;
    }
}
