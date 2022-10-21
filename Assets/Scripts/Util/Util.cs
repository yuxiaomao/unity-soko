using UnityEngine;

public class Util
{
    public static Vector2Int Vector3toVector2IntXZ(Vector3 v3)
    {
        return new Vector2Int((int)v3.x, (int)v3.z);
    }

    public static Vector3 Vector2InttoVector3XZ(Vector2Int v2i, float y = 0)
    {
        return new Vector3(v2i.x, y, v2i.y);
    }
}
