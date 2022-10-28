using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global control for the current level
/// </summary>
public class LevelController : MonoBehaviour
{
    [Serializable]
    private class MoveRecord
    {
        public readonly Vector3 direction;
        public readonly ObjectController.MoveResult[] playersResult;

        public MoveRecord(Vector3 direction, int playerCount)
        {
            this.direction = direction;
            playersResult = new ObjectController.MoveResult[playerCount];
        }
    }

    private static LevelController Instance { get; set; }
    private PlayerController[] players;
    private TargetController[] targets;
    private int totalTarget;
    private readonly Stack<MoveRecord> history = new();

    private void Start()
    {
        GameManager.AssertIsChild(gameObject);
        Instance = this;
    }

    public void InitCurrentLevelReferences()
    {
        players = Util.GetComponentsFromObjects<PlayerController>(GameObject.FindGameObjectsWithTag(Constants.TagPlayer));
        targets = Util.GetComponentsFromObjects<TargetController>(GameObject.FindGameObjectsWithTag(Constants.TagTarget));
        totalTarget = targets.Length;
        history.Clear();
    }

    /// <summary>
    /// Move all player objects, called by UserInputManager
    /// </summary>
    /// <param name="direction"></param>
    public static void LevelMoveAllPlayers(Vector3 direction)
    {
        MoveRecord currentRecord = new(direction, Instance.players.Length);
        int unknownMoveCount;
        int moveCount = 0;
        do
        {
            unknownMoveCount = 0;
            for (int i = 0; i < Instance.players.Length; i++)
            {
                if (currentRecord.playersResult[i] == null ||
                    currentRecord.playersResult[i].state == ObjectController.MoveState.Unknown)
                {
                    currentRecord.playersResult[i] = Instance.players[i].TryMove(direction, true);
                    if (currentRecord.playersResult[i].state == ObjectController.MoveState.Unknown)
                    {
                        unknownMoveCount += 1;
                    }
                    if (currentRecord.playersResult[i].state == ObjectController.MoveState.WillMove)
                    {
                        moveCount += 1;
                    }
                }
            }
        } while (unknownMoveCount != 0);
        Instance.history.Push(currentRecord);
        if (moveCount > 0)
        {
            AudioManager.PlaySE(AudioManager.SE.Move);
        }
        else
        {
            AudioManager.PlaySE(AudioManager.SE.Error);
        }
        GameManager.ShouldUpdateGameState();
    }

    /// <summary>
    /// Undo move for all player objects, called by UserInputManager
    /// </summary>
    public static void LevelUndoAllPlayers()
    {
        // If does not have history
        if (Instance.history.Count <= 0)
        {
            AudioManager.PlaySE(AudioManager.SE.Error);
            return;
        }

        // If does have history
        MoveRecord currentRecord = Instance.history.Pop();
        for (int i = 0; i < Instance.players.Length; i++)
        {
            Instance.players[i].UndoMove(currentRecord.direction, currentRecord.playersResult[i]);
        }
        AudioManager.PlaySE(AudioManager.SE.Move);
    }

    public bool LevelIsWin()
    {
        int withBoxTarget = 0;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].IsTargetWithBox())
            {
                withBoxTarget += 1;
            }
        }
        return withBoxTarget == totalTarget;
    }

}
