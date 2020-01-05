using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    [Header("Keyboard Inputs")]
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public KeyCode interact;

    private SpriteRenderer spriteCursor;
    private Collider2D cursorCollider;
    private Rigidbody2D rb;
    public float cursorSpeed = 50f;

    [SerializeField]
    private bool didInteractThisFrame = false;
    public bool DidInteractThisFrame { get { return didInteractThisFrame; } }

    [SerializeField]
    private Vector2 direction;

    public PlayerInfo playerInfo;

    void Awake()
    {
        spriteCursor = GetComponent<SpriteRenderer>();
        cursorCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector2.zero;
        if(Input.GetKey(up))
        {
            direction.y = 1f;
        }
        else if(Input.GetKey(down))
        {
            direction.y = -1f;
        }

        if (Input.GetKey(left))
        {
            direction.x = -1f;
        }
        else if(Input.GetKey(right))
        {
            direction.x = 1f;
        }

        if(Input.GetKeyDown(interact))
        {
            // notify something was pressed on
            didInteractThisFrame = true;
        }

        direction = direction.normalized;
        Vector3 finalSpeed = new Vector3(direction.normalized.x, direction.normalized.y, 0f) * cursorSpeed * Time.deltaTime;
        transform.Translate(finalSpeed);
    }

    public void OnInteractWith(GameObject boardPiece, BoardController gameboard)
    {
        // call this on boardPiece when this gameObject's component triggers an OnTriggerStay event
        if(didInteractThisFrame)
        {
            didInteractThisFrame = false;
            // change gameobject's color, set its lifespan, and update its neighboring pieces lifespans if neighboring piece is the same color as boardPiece
            boardPiece.GetComponent<SpriteRenderer>().color = playerInfo.color;
            //BoardController gameboard = boardPiece.GetComponent<BoardPiece>().Board;

            int row = Mathf.FloorToInt(boardPiece.transform.position.y / gameboard.BoardPieceSize);
            int col = Mathf.FloorToInt(boardPiece.transform.position.x / gameboard.BoardPieceSize);

            gameboard.timeSpanBoard[row][col] = playerInfo.markerLifeSpan;
            gameboard.markerBoard[row][col] = playerInfo.marker;

            // iteratively as long as they're in the same axis they update
            gameboard.ExtendNeighboringPiecesLifespans(row, col, playerInfo);

            //BoardPiece.Neighbors neighbors = boardPiece.GetComponent<BoardPiece>().IdentifyImmediateNeighbors(row, col, playerInfo);
            //gameboard.ExtendNeighboringPiecesLifespansRecursively(neighbors, playerInfo);

            gameboard.PrintBoards();
        }
    }
}
