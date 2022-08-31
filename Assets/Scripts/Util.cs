using UnityEngine;

public static class Util
{
    public static float ToBounds(this float val, float min, float max)
    {
        return Mathf.Min(Mathf.Max(val, min), max);
    }

    public static bool InBounds(this float val, float min, float max)
    {
        return val >= min && val <= max;
    }
}
