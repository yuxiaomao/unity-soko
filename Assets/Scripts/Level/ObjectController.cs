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

    public class MoveResult
    {
        public readonly MoveState state;
        public readonly bool pushed;

        public MoveResult(MoveState state, bool pushed = false)
        {
            this.state = state;
            this.pushed = pushed;
        }
    }

    private const float MoveCheckDistance = 1.0f;
    private int lastStateUpdateFrame = -1;
    private MoveState m_CurrentState;
    public MoveState CurrentState
    {
        get
        {
            return lastStateUpdateFrame != Time.frameCount ? MoveState.Unknown : m_CurrentState;
        }
        private set
        {
            lastStateUpdateFrame = Time.frameCount;
            m_CurrentState = value;
        }
    }

    /// <summary>
    /// Try to move to the given direction
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="canPush">can push box</param>
    /// <returns></returns>
    public MoveResult TryMove(Vector3 direction, bool canPush = false)
    {
        bool pushed = false;
        // Check Collision object in front of object's direction
        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, MoveCheckDistance))
        {
            // Has collision
            GameObject other = hitInfo.collider.gameObject;

            if (other.CompareTag(Constants.TagWall))
            {
                // Collision with wall
                return new MoveResult(CurrentState = MoveState.NoMove);
            }
            else if (other.CompareTag(Constants.TagBox))
            {
                // Collision with box

                // Collision with box but can not push the box
                if (!canPush)
                {
                    return new MoveResult(CurrentState = MoveState.NoMove);
                }

                // Collision with Box and can push, try to move the Box
                MoveState result = other.GetComponent<BoxController>().TryMove(direction).state;
                if (result != MoveState.WillMove)
                {
                    return new MoveResult(CurrentState = result);
                }
                // else WillMove
                pushed = true;
            }
            else if (other.CompareTag(Constants.TagPlayer))
            {
                // Collision with Player
                MoveState playerState = other.GetComponent<PlayerController>().CurrentState;
                if (playerState != MoveState.WillMove)
                {
                    return new MoveResult(CurrentState = playerState);
                }
                // else WillMove
            }
        }

        // No collision or WillMove
        transform.position += direction;
        return new MoveResult(CurrentState = MoveState.WillMove, pushed);
    }

    /// <summary>
    /// Undo a move with its original move direction and result
    /// </summary>
    /// <param name="direction">original move direction</param>
    /// <param name="moveResult">original move result</param>
    public void UndoMove(Vector3 direction, MoveResult moveResult = default)
    {
        switch (moveResult.state)
        {
            case MoveState.NoMove:
                {
                    return;
                }
            case MoveState.WillMove:
                {
                    if (moveResult.pushed)
                    {
                        if (Physics.Raycast(transform.position, direction, out RaycastHit hitInfo, MoveCheckDistance))
                        {
                            GameObject other = hitInfo.collider.gameObject;
                            if (other.CompareTag(Constants.TagBox))
                            {
                                other.GetComponent<BoxController>().UndoMove(direction, new MoveResult(MoveState.WillMove));
                            }
                            else
                            {
                                Debug.LogError("Try to undo a push but pushed object is not a box");
                            }
                        }
                        else
                        {
                            Debug.LogError("Try to undo a push but can not found pushed object");
                        }

                    }
                    transform.position -= direction;
                    return;
                }
            default:
                {
                    Debug.LogError("Unknown MoveResult");
                    return;
                }
        }
    }
}
