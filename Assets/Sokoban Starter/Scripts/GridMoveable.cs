using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveable : MonoBehaviour
{
    // The naming of this script need to be changed.
    // This one should be called as gridobject, but it's already taken.
    // They could merge into 1 script...
    // But I'm not allowed to modify that script...
    public GridObject gridObject;
    public Vector2Int movingDirection = new(0, 0);
    public Vector2Int tryingDirection = new(0, 0);

    public bool moveSuccessed;
    // Start is called before the first frame update
    void Start()
    {
        this.gridObject = this.GetComponent<GridObject>();
    }

    public bool Move(Vector2Int target, bool record = true)
    {
        void successfulMove()
        {
            movingDirection = target;
            if (record) { GameManager.Instance.movedBlocks.Add(this); }
        }
        // Check with the boundary first
        if ((this.gridObject.gridPosition + target).x < 1 ||
            (this.gridObject.gridPosition + target).y < 1 ||
            (this.gridObject.gridPosition + target).x > FindObjectOfType<GridMaker>().dimensions.x ||
            (this.gridObject.gridPosition + target).y > FindObjectOfType<GridMaker>().dimensions.y)
        { return false; }

        else
        {
            // Check if the destinated location have a block
            List<GridMoveable> matchingBlock = new();
            foreach (GridMoveable block in FindObjectsOfType<GridMoveable>())
            {
                if (block.gridObject.gridPosition + block.movingDirection == this.gridObject.gridPosition + target)
                {
                    if (block != this) { matchingBlock.Add(block); }
                    print(block);
                }
            }

            if (matchingBlock.Count > 0)
            {
                // Here means we have a block in our way.
                // We need to check if it is a wall first, this will save us a lot time.
                if (matchingBlock[0].CompareTag("Wall") || matchingBlock[0].CompareTag("Clingy"))
                {
                    return false;
                }
                else
                {
                    if (matchingBlock[0].Move(target))
                    {
                        successfulMove();
                        return true;
                    }
                    else
                    {
                        tryingDirection = target;
                        return false;
                    }
                }
            }
            else
            {
                successfulMove();
                return true;
            }
        }
    }
}
