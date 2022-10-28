using System;
using System.Reflection;
using UnityEngine;

public static class Util
{

    public static Vector2Int Vector3toVector2IntXZ(Vector3 v3)
    {
        return new Vector2Int((int)v3.x, (int)v3.z);
    }

    public static Vector3 Vector2InttoVector3XZ(Vector2Int v2i, float y = 0)
    {
        return new Vector3(v2i.x, y, v2i.y);
    }

    /// <summary>
    /// Will get the string value attribute assigned for a given enums value,
    /// if not present, return enum.ToString()
    /// </summary>
    /// <param name="value">Enum marked with attribute [StringValue("a")]</param>
    /// <returns></returns>
    public static string GetEnumStringValue(this Enum value)
    {
        // Get the type
        Type type = value.GetType();

        // Get fieldinfo for this type
        FieldInfo fieldInfo = type.GetField(value.ToString());

        // Get the stringvalue attributes
        StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
            typeof(StringValueAttribute), false) as StringValueAttribute[];

        // Return the first if there was a match.
        return attribs.Length > 0 ? attribs[0].StringValue : value.ToString();
    }

    /// <summary>
    /// Clone a serializable object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T Clone<T>(this T source)
    {
        string serialized = JsonUtility.ToJson(source);
        return JsonUtility.FromJson<T>(serialized);
    }

    /// <summary>
    /// Convert array of game objects to array of components (e.g. PlayerController)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gos"></param>
    /// <returns></returns>
    public static T[] GetComponentsFromObjects<T>(GameObject[] gos) where T : Component
    {
        T[] components = new T[gos.Length];
        for (int i = 0; i < gos.Length; i++)
        {
            components[i] = gos[i].GetComponent<T>();
        }
        return components;
    }
}
