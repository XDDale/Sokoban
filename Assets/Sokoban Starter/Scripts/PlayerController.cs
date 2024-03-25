using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isFrozen = false;
    private GridMoveable gridMoveable;
    // Start is called before the first frame update
    void Start()
    {
        this.gridMoveable = this.GetComponent<GridMoveable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFrozen)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.BackToStart();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                GameManager.Instance.Redo();
            }
            else
            {
                this.isFrozen = true;
                if (Input.GetKeyDown(KeyCode.W))
                {
                    GameManager.Instance.Record();
                    this.gridMoveable.Move(new Vector2Int(0, -1));
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    GameManager.Instance.Record();
                    this.gridMoveable.Move(new Vector2Int(0, 1));
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    GameManager.Instance.Record();
                    this.gridMoveable.Move(new Vector2Int(-1, 0));
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    GameManager.Instance.Record();
                    this.gridMoveable.Move(new Vector2Int(1, 0));
                }
            }
        }
    }
}
