using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static float ToBounds(float val, float min, float max)
    {
        return Mathf.Min(Mathf.Max(val, min), max);
    }

    public static bool InBounds(float val, float min, float max)
    {
        return val >= min && val <= max;
    }
}
