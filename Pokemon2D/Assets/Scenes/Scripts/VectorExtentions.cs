using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtentions 
{
    private static List<Vector2> compass2D = new List<Vector2>() { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

    public static Vector2 Generalize(this Vector2 org)
    {
        float maxDot = -Mathf.Infinity;
        Vector2 ret = Vector2.zero;

        if (org == ret)
        {
            return ret;
        }

        foreach (Vector2 dir in compass2D)
        {
            var t = Vector2.Dot(org, dir);
            if (t > maxDot)
            {
                ret = dir;
                maxDot = t;
            }
        }
        return ret;
    }

}
