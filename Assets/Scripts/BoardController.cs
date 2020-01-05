using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{
    public const int FREE_SPOT = 0;

    public float current_time;
    public GlobalTimer timer;
    public GameObject boardPiecePrefab;
    public int dimension;
    [SerializeField]
    private float boardPieceLength;
    public float padding = 10f;
    public float BoardPieceSize { get { return boardPieceLength + padding;  } }

    private GameObject[][] boardPieceGOs;
    public GameObject[][] BoardPieceGOs { get { return boardPieceGOs; } }
    private BoardPiece[][] boardPieces;
    public BoardPiece[][] BoardPieces { get { return boardPieces; } }
    public float[][] timeSpanBoard; // measured in seconds
    public int[][] markerBoard;
    public int[][] checkBoard;



    public KeyboardController player1;
    public KeyboardController player2;

    private static BoardController instance = null;
    public static BoardController Instance { get { return instance; } }

    public int turn = 1;

    void Awake()
    {
        // only need to worry about destroying the singleton instance if multiple scenes are involved....
        instance = this;

        timeSpanBoard = new float[dimension][];
        markerBoard = new int[dimension][];
        checkBoard = new int[dimension][];

        for (int r = 0;r < dimension; ++r)
        {
            timeSpanBoard[r] = new float[dimension];
            markerBoard[r] = new int[dimension];
            checkBoard[r] = new int[dimension];

            for (int c = 0;c < dimension; ++c)
            {
                timeSpanBoard[r][c] = 0f;
                markerBoard[r][c] = FREE_SPOT;
                checkBoard[r][c] = 0;
            }
        }

        //for (int r = 0; r < dimension; ++r)
        //{
        //    for (int c = 0; c < dimension; ++c)
        //    {
        //        checkBoard[r][c] = 0;
        //    }
        //}

        //setting time
        current_time = Time.time;

        // debugging
        PrintBoards();

        boardPieceLength = boardPiecePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        boardPieceGOs = new GameObject[dimension][];
        boardPieces = new BoardPiece[dimension][];
        for(int r = 0;r < dimension; ++r)
        {
            boardPieceGOs[r] = new GameObject[dimension];
            boardPieces[r] = new BoardPiece[dimension];
            for(int c = 0;c < dimension; ++c)
            {
                boardPieceGOs[r][c] = Instantiate(boardPiecePrefab, 
                    new Vector3(c * (boardPieceLength + padding), r * (boardPieceLength + padding)), 
                    transform.rotation, transform);

                boardPieces[r][c] = boardPieceGOs[r][c].GetComponent<BoardPiece>();
                boardPieces[r][c].SetCoords(r, c);
            }
        }
    }


    public List<BoardPiece> getAllNeighbors(int row, int col)
    {
        List<BoardPiece> return_bps = new List<BoardPiece>();
        int original_row = row;
        int original_col = col;
        int[,] offsets = {{ 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 }};
        for (int i = 0; i < 4; i++)
        {
            row = original_row + offsets[i,0];
            col = original_col + offsets[i,1];
            if (!(row < 0 || row >= dimension || col < 0 || col >= dimension))
            {
                if (markerBoard[row][col] == turn && checkBoard[row][col] == 0)
                {
                    return_bps.Add(BoardPieces[row][col]);
                    checkBoard[row][col] = 1;
                    foreach (BoardPiece bp in getAllNeighbors(row, col))
                    {
                        return_bps.Add(bp);
                    }
                    
                }
            }
        }
        return return_bps;
        
    }

    private void UpdateBoard(float deltaTime)
    {
        for(int r = 0;r < dimension; ++r)
        {
            for(int c = 0;c < dimension; ++c)
            {
                float secondsLeft = timeSpanBoard[r][c];
                int currentMarker = markerBoard[r][c];
                if (currentMarker == FREE_SPOT) continue;

                secondsLeft -= deltaTime;
                if(secondsLeft <= 0f)
                {
                    currentMarker = FREE_SPOT;
                    secondsLeft = 0f;
                    boardPieceGOs[r][c].GetComponent<SpriteRenderer>().color = Color.white;
                }

                markerBoard[r][c] = currentMarker;
                timeSpanBoard[r][c] = secondsLeft;
                boardPieces[r][c].TimerText.text = Mathf.CeilToInt(secondsLeft).ToString();
            }
        }

    }

    void Update()
    {
        if(Time.time - current_time >= 2)
        {
            current_time = Time.time;
            turn = turn == 1 ? 2 : 1;
            //timer.GetComponent<Text>().color = turn == 1 ? Color.green : Color.cyan; //new Color(0, 255, 26) : new Color(0, 180, 255);
            Camera.main.backgroundColor = turn == 1 ? Color.green : Color.cyan; //new Color(0, 255, 26) : new Color(0, 180, 255);
        }

        //// 1. determine who won
        ////      -> counting number of colored squares per player
        /////     -> rearrange colors by having winner's colors appear first on top left corner of screen
        /////     -> place loser's color as remaining slots to fill on board
        ///// 2. Add replay button to show up on screen
        ///// 
        //if (timer.currentTime < 0)
        //{
        //    int one_counter = 0;
        //    int two_counter = 0;

        //    for (int r = 0; r < dimension; r++)
        //    {
        //        for (int c = 0; c < dimension; c++)
        //        {
        //            if (markerBoard[r][c] == 1) {
        //                one_counter += 1;
        //            }
        //            else if (markerBoard[r][c] == 2)
        //            {
        //                two_counter += 1;
        //            }
        //        }
        //    }
        //    int winner = one_counter > two_counter ? 1 : 2;
        //    for(int r = 0; r < dimension; r++)
        //    {
        //        for(int c = 0; c < dimension; c++)
        //        {
        //            if(winner == 1)
        //            {
        //                markerBoard
        //            }
        //        }
        //    }
        //}

        
        //}
        UpdateBoard(Time.deltaTime);


    }

    public void PrintBoards()
    {
        string s1 = "";
        string s2 = "";
        for (int r = 0; r < dimension; ++r)
        {
            for (int c = 0; c < dimension; ++c)
            {
                s1 += string.Format("--{0}--", timeSpanBoard[r][c]);
                s2 += markerBoard[r][c].ToString();
            }

            s1 += "\n";
            s2 += "\n";
        }

        Debug.Log("TimeSpanBoard");
        Debug.Log(s1);
        Debug.Log("MarkerBoard");
        Debug.Log(s2);
    }

    public void ExtendNeighboringPiecesLifespans(int row, int col, PlayerInfo info)
    {
        // check all top neighbors
        for (int r = row + 1; r < dimension; ++r) 
        {
            if (markerBoard[r][col] == FREE_SPOT) break;
            if (markerBoard[r][col] != info.marker) break;
            timeSpanBoard[r][col] = info.markerLifeSpan;
        }

        // check all bottom neighbors
        for (int r = row - 1; r >= 0; --r) 
        {
            if (markerBoard[r][col] == FREE_SPOT) break;
            if (markerBoard[r][col] != info.marker) break;
            timeSpanBoard[r][col] = info.markerLifeSpan;
        }

        // check all right neighbors
        for(int c = col + 1; c < dimension; ++c) 
        {
            if (markerBoard[row][c] == FREE_SPOT) break;
            if (markerBoard[row][c] != info.marker) break;
            timeSpanBoard[row][c] = info.markerLifeSpan;
        }

        // check all left neighbors
        for(int c = col - 1; c >= 0; --c) 
        {
            if (markerBoard[row][c] == FREE_SPOT) break;
            if (markerBoard[row][c] != info.marker) break;
            timeSpanBoard[row][c] = info.markerLifeSpan;
        }
    }

    public void ExtendNeighboringPiecesLifespansRecursively(BoardPiece.Neighbors neighbors, PlayerInfo info)
    {
        ExtendLifeSpan(neighbors.Left, info);
        ExtendLifeSpan(neighbors.Right, info);
        ExtendLifeSpan(neighbors.Up, info);
        ExtendLifeSpan(neighbors.Down, info);
    }

    private void ExtendLifeSpan(BoardPiece piece, PlayerInfo info)
    {
        if (piece == null)
            return;

        if (markerBoard[piece.row][piece.col] != info.marker)
            return;
        //if (piece.SpriteRenderer.color != info.color)
        //    return;

        timeSpanBoard[piece.row][piece.col] = info.markerLifeSpan;

        var neighbors = piece.IdentifyImmediateNeighbors(piece.row, piece.col, info);
        ExtendNeighboringPiecesLifespansRecursively(neighbors, info);
    }
}
