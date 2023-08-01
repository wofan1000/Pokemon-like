using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectDB<T> : MonoBehaviour where T : ScriptableObject
{
    static Dictionary<string, T> objects;

    public static void init()
    {
        objects = new Dictionary<string, T>();

        var objectArrey = Resources.LoadAll<T>("");
        foreach (var obj in objectArrey)
        {
            if (objects.ContainsKey(obj.name))
            {
                continue;
            }


            objects[obj.name] = obj;
        }
    }
    public static T GetObjectbyName(string name)
    {
        if (!objects.ContainsKey(name))
        {
            return null;
        }
        return objects[name];
    }
}


