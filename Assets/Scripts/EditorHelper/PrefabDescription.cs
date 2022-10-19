#if UNITY_EDITOR
using UnityEngine;

public class PrefabDescription : MonoBehaviour
{
    [TextArea(5, 20)]
    public string description = "";
}
#endif