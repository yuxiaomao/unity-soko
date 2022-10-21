using UnityEngine;

public class TargetController : ObjectController
{
    private const float TargetCheckRadius = 0.1f;

    /// <summary>
    /// Return true if the given target is occupied by a box
    /// </summary>
    /// <returns></returns>
    public bool IsTargetWithBox()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, TargetCheckRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.CompareTag(Constants.TagBox))
            {
                return true;
            }
        }
        return false;
    }
}
