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

    public static bool InDelta(this float val, float x, float delta)
    {
        return val >= x - delta && val <= x + delta;
    }
}
