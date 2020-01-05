using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

public class BoardPiece : MonoBehaviour
{
    public class Neighbors
    {
        public BoardPiece Left { get; set; }
        public BoardPiece Right { get; set; }
        public BoardPiece Up { get; set; }
        public BoardPiece Down { get; set; }

        public Neighbors() { }
    }

    [SerializeField]
    private BoardController board;
    public BoardController Board { get { return board; } }
    
    private TextMeshPro timerText;
    public TextMeshPro TimerText { get { return timerText; } }

    public int row = -1, col = -1;
    public void SetCoords(int newRow, int newCol)
    {
        row = newRow;
        col = newCol;
    }

    private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }

    void Awake()
    {
        board = FindObjectOfType<BoardController>();
        Assert.IsNotNull(board);

        timerText = transform.Find("timer").GetComponent<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<KeyboardController>();
        if(player != null)
        {
            player.OnInteractWith(gameObject, BoardController.Instance);
        } 
    }

    public Neighbors IdentifyImmediateNeighbors(int row, int col, PlayerInfo info)
    {
        if (row < 0 || row >= board.dimension || col < 0 || col >= board.dimension)
            return new Neighbors();

        return new Neighbors()
        {
            Left = col - 1 >= 0 &&
                board.markerBoard[row][col - 1] == info.marker ?
                board.BoardPieces[row][col - 1] :
                null,
            Right = col + 1 < board.dimension &&
                board.markerBoard[row][col + 1] == info.marker ?
                board.BoardPieces[row][col + 1] :
                null,
            Up = row - 1 >= 0 &&
                board.markerBoard[row - 1][col] == info.marker ?
                board.BoardPieces[row - 1][col] :
                null,
            Down = row + 1 < board.dimension &&
                board.markerBoard[row + 1][col] == info.marker ?
                board.BoardPieces[row + 1][col] :
                null
        };
    }
}
