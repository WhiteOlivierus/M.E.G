using UnityEngine;

public static class Utilities
{
    public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    public static Vector3 ClampVector(Vector3 toClamp, Vector3 min, Vector3 max)
    {
        toClamp.x = Mathf.Clamp(toClamp.x, min.x, max.x);
        toClamp.y = Mathf.Clamp(toClamp.y, min.y, max.y);
        toClamp.z = Mathf.Clamp(toClamp.z, min.z, max.z);
        return toClamp;
    }
}
