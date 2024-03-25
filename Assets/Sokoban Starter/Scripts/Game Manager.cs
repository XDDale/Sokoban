using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GridMoveable> movedBlocks = new();
    public static GameManager Instance { get; private set; }
    private List<GridMoveable> checkedMovedBlocks = new();

    public Stack<Dictionary<GridObject, Vector2Int>> thePast = new();

    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        if (movedBlocks.Count > 0)
        {
            List<GridMoveable> newMovedBlocks = new();
            GridMoveable[] allBlocks = FindObjectsOfType<GridMoveable>();

            foreach (GridMoveable block in allBlocks)
            {
                if (block.tryingDirection != new Vector2Int(0, 0))
                {
                    block.Move(block.tryingDirection);
                }
            }

            foreach (GridMoveable movedBlock in movedBlocks)
            {
                foreach (GridMoveable block in allBlocks)
                {
                    if (!checkedMovedBlocks.Contains(movedBlock))
                    {
                        if (block.CompareTag("Sticky"))
                        {
                            if ((block.gridObject.gridPosition - movedBlock.gridObject.gridPosition).magnitude <= 1)
                            {
                                block.Move(movedBlock.movingDirection, record: false);
                                newMovedBlocks.Add(block);
                            }
                        }
                        else if (block.CompareTag("Clingy"))
                        {
                            if (block.gridObject.gridPosition == movedBlock.gridObject.gridPosition - movedBlock.movingDirection)
                            {
                                block.Move(movedBlock.movingDirection, record: false);
                                newMovedBlocks.Add(block);
                            }
                        }
                    }
                }
                checkedMovedBlocks.Add(movedBlock);
            }
            movedBlocks = newMovedBlocks;
        }
        else
        {
            checkedMovedBlocks = new();
            foreach (GridMoveable block in FindObjectsOfType<GridMoveable>())
            {
                block.gridObject.gridPosition += block.movingDirection;
                block.tryingDirection = new Vector2Int(0, 0);
                block.movingDirection = new Vector2Int(0, 0);
            }
            foreach (PlayerController player in FindObjectsOfType<PlayerController>())
            {
                player.isFrozen = false;
            }
        }
    }

    public void Record()
    {
        Dictionary<GridObject, Vector2Int> objectsPositions = new();
        foreach (GridObject gridObject in FindObjectsOfType<GridObject>())
        {
            objectsPositions.Add(gridObject, gridObject.gridPosition);
        }
        thePast.Push(objectsPositions);
        foreach (KeyValuePair<GridObject, Vector2Int> keyValuePair in objectsPositions)
        {
            print(keyValuePair.Key.ToString() + keyValuePair.Value.ToString());
        }
    }

    public void Redo()
    {
        if (thePast.Count != 0)
        {
            Dictionary<GridObject, Vector2Int> nextFuture = thePast.Pop();
            foreach (KeyValuePair<GridObject, Vector2Int> pair in nextFuture)
            {
                pair.Key.gridPosition = pair.Value;
            }
        }
    }

    public void BackToStart()
    {
        if (thePast.Count != 0)
        {
            Dictionary<GridObject, Vector2Int> nextFuture = null;
            while (thePast.Count > 0)
            {
                nextFuture = thePast.Pop();
            }
            foreach (KeyValuePair<GridObject, Vector2Int> pair in nextFuture)
            {
                pair.Key.gridPosition = pair.Value;
            }
        }
    }
}
