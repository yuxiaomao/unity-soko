using UnityEngine;

/// <summary>
/// Environement objects base class, objects should have thier class inherits from this class
/// </summary>
public class ObjectController : MonoBehaviour
{
    public enum MoveState
    {
        Unknown,
        WillMove,
        NoMove,
    }

    private const float MoveCheckDistance = 1.0f;

    private int lastStateUpdateFrame = -1;
    private MoveState m_NextState;
    public MoveState NextState
    {
        get
        {
            return lastStateUpdateFrame != Time.frameCount ? MoveState.Unknown : m_NextState;
        }
        private set
        {
            lastStateUpdateFrame = Time.frameCount;
            m_NextState = value;
        }
    }

    /// <summary>
    /// Try to move to the given direction
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="canPush">can push box</param>
    /// <returns></returns>
    public MoveState TryMove(Vector3 direction, bool canPush = false)
    {
        // Check Collision object in front of object's direction
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, MoveCheckDistance))
        {
            // Has collision
            GameObject other = hitInfo.collider.gameObject;

            if (other.CompareTag(Constants.TagWall))
            {
                // Collision with wall
                return NextState = MoveState.NoMove;
            }
            else if (other.CompareTag(Constants.TagBox))
            {
                // Collision with box

                // Collision with box but can not push the box
                if (!canPush)
                {
                    return NextState = MoveState.NoMove;
                }

                // Collision with Box and can push, try to move the Box
                MoveState result = other.GetComponent<BoxController>().TryMove(direction);
                if (result != MoveState.WillMove)
                {
                    return NextState = result;
                }
                // else WillMove
            }
            else if (other.CompareTag(Constants.TagPlayer))
            {
                // Collision with Player
                MoveState playerState = other.GetComponent<PlayerController>().NextState;
                if (playerState != MoveState.WillMove)
                {
                    return NextState = playerState;
                }
                // else WillMove
            }
        }

        // No collision or WillMove
        transform.position += direction;
        return NextState = MoveState.WillMove;
    }
}
