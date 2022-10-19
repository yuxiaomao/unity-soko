using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float moveCheckDistance = 1.0f;

    /// <summary>
    /// Move player to the given direction
    /// </summary>
    public void Move(Vector3 direction)
    {
        // Check Collision object in front of player's direction
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, direction, out hitInfo, moveCheckDistance))
        {
            // Has collision
            GameObject other = hitInfo.collider.gameObject;
            if (other.CompareTag("Wall"))
            {
                // Collision with wall, can not move
                AudioManager.PlaySEError();
                return;
            }
            if (other.CompareTag("Box"))
            {
                // Collision with Box, try to move the Box
                if (!other.GetComponent<BoxController>().TryMove(direction))
                {
                    // Box not moved
                    AudioManager.PlaySEError();
                    return;
                }
            }
        }

        // No collision or box is pushed, player move
        transform.position += direction;
        GameManager.ShouldUpdateGameState();
        AudioManager.PlaySEMove();
    }
}
