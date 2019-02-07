using UnityEngine;
using UnityEngine.UI;

public class SliderComponent : MonoBehaviour
{
    public Transform min, max;
    public float value;
    public Vector2 range;
    public Text monitor;

    public float position = 0f;
    public Vector3 minV, maxV;

    private void Start()
    {
        minV = min.position;
        maxV = max.position;
    }

    void Update()
    {
        position = Mathf.Clamp(position, 0f, 1f);
        transform.position = Vector3.Lerp(minV, maxV, position);

        float dist = Vector3.Distance(minV, maxV);
        float distToMin = Vector3.Distance(minV, transform.position);

        float t = dist - distToMin;

        value = (int)Map(t, 0, dist, range.x, range.y);
        monitor.text = value.ToString();
    }

    public void ChangePosition(int move)
    {
        position += move / 100f;
    }

    public float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}
