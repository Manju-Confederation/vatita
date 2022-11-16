using UnityEngine;

public static class Util
{
    public static bool InBounds(this float val, float min, float max)
    {
        return val >= min && val <= max;
    }

    public static bool InDelta(this float val, float x, float delta)
    {
        return val >= x - delta && val <= x + delta;
    }

    public static float Delta(this float val, float x)
    {
        return Mathf.Abs(val - x);
    }
}
