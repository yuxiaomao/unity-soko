using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float MoveCheckDistance = 1.0f;

    /// <summary>
    /// Move player to the given direction
    /// </summary>
    public void Move(Vector3 direction)
    {
        // Check Collision object in front of player's direction
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, MoveCheckDistance))
        {
            // Has collision
            GameObject other = hitInfo.collider.gameObject;
            if (other.CompareTag(Constants.TagWall))
            {
                // Collision with wall, can not move
                AudioManager.PlaySE(AudioManager.SE.Error);
                return;
            }
            if (other.CompareTag(Constants.TagBox))
            {
                // Collision with Box, try to move the Box
                if (!other.GetComponent<BoxController>().TryMove(direction))
                {
                    // Box not moved
                    AudioManager.PlaySE(AudioManager.SE.Error);
                    return;
                }
            }
        }

        // No collision or box is pushed, player move
        transform.position += direction;
        GameManager.ShouldUpdateGameState();
        AudioManager.PlaySE(AudioManager.SE.Move);
    }
}
