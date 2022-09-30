using UnityEngine;

public class TargetController : ObjectController
{
    private const float targetCheckRadius = 0.1f;

    // Return true if the given target is occupied by a box
    public bool IsTargetWithBox()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, targetCheckRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Box"))
            {
                return true;
            }
        }
        return false;
    }
}
