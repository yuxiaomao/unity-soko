using UnityEngine;

public class BoxController : ObjectController
{
    private const float moveCheckDistance = 1.0f;

    /// <summary>
    /// Return true if move can and is done, false if can not move
    /// </summary>
    public bool TryMove(Vector3 direction)
    {
        // A box can move if nothing in front
        if (!Physics.Raycast(transform.position, direction, moveCheckDistance))
        {
            // No collision, move on the given direction
            transform.position += direction;
            return true;
        }

        // In default case do not move
        return false;
    }
}
