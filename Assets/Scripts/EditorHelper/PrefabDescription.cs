using UnityEngine;

public class PrefabDescription : MonoBehaviour
{
#if UNITY_EDITOR
    [TextArea(5, 20)]
    public string description = "";
#endif
}
