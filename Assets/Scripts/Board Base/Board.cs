using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width = 8;
    public int height = 9;

    public int borderSize = 1;

    public GameObject cellPrefab;

    // reference to a Bomb created on the clicked Tile (first Tile clicked by mouse or finger)
    public GameObject clickedTileBomb;
	
    // reference to a Bomb created on the target Tile (Tile dragged into by mouse or finger)
    public GameObject targetTileBomb;

    // the time required to swap GamePieces between the Target and Clicked Tile
    public float swapTime = 0.5f;
    
    Cell[,] allTiles;
    public GamePiece[,] allGamePieces;

    public Cell clickedCell;

    public Cell targetCell;

    // whether user input is currently allowed
    public bool playerInputEnabled = true;

    public ParticleManager particleManager;



    // the current score multiplier, depending on how many chain reactions we have caused
    public int scoreMultiplier = 0;

    public bool isRefilling = false;

    public BoardDeadlock boardDeadlock;
    public BoardShuffler boardShuffler;
    public BoardFiller boardFiller;
    public MatchFinder matchFinder;
    public FallManager fallManager;
    public ItemManager ıtemManager;
    public ItemFactory itemFactory;
    public float delay = 0.2f;
    public SwapManager swapManager;
    public ClearManager clearManager;
    public BoardManager boardManager;

    private void Awake()
    {

        boardDeadlock = GetComponent<BoardDeadlock>();
        boardShuffler = GetComponent<BoardShuffler>();
        boardFiller = GetComponent<BoardFiller>();
        matchFinder = GetComponent<MatchFinder>();
        fallManager = GetComponent<FallManager>();
        ıtemManager = GetComponent<ItemManager>();
        itemFactory = GetComponent<ItemFactory>();
        swapManager = GetComponent<SwapManager>();
        clearManager = GetComponent<ClearManager>();
        boardManager = GetComponent<BoardManager>();
    }

    void Start()
    {
        allTiles = new Cell[width, height];
        allGamePieces = new GamePiece[width, height];
        particleManager = GameObject.FindWithTag("ParticleManager").GetComponent<ParticleManager>();
    }
    
    public void SetupBoard()
    {
        SetupCells();
        SetupCamera();

        FillBoard();
    }
    
    public void SetupCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allTiles[x, y] == null)
                {
                    GameObject tile = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
                    allTiles[x, y] = tile.GetComponent<Cell>();
                    allTiles[x, y].Init(x, y, this);
                }
            }
        }
    }
    public void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((float)(width - 1) / 2f, (float)(height - 1) / 2f, -10f);

        float aspectRatio = (float)Screen.width / (float)Screen.height;

        float verticalSize = (float)height / 2f + (float)borderSize;

        float horizontalSize = ((float)width / 2f + (float)borderSize) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
    }
    
    public void FillBoard()
    {
        int maxIterations = 100;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {           
                if (allGamePieces[i, j] == null)
                {
                    itemFactory.MakeRandomGamePiece(i, j);
                    int iteration = 0;

                    while (HasMatchOnFill(i, j) && iteration < maxIterations)
                    {
                        clearManager.DestroyAt(i, j);
                        itemFactory.MakeRandomGamePiece(i, j);

                        iteration++;
                    }
                }
            }
        }
    }
    
    public bool IsInBorder(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    public bool IsNextTo(Cell start, Cell end)
    {
        if (Mathf.Abs(start.xIndex - end.xIndex) == 1 && start.yIndex == end.yIndex)
        {
            return true;
        }

        if (Mathf.Abs(start.yIndex - end.yIndex) == 1 && start.xIndex == end.xIndex)
        {
            return true;
        }
        return false;    }
    
    public bool HasMatchOnFill(int x, int y)
    {
        if (x > 1)
        {
            MatchValue matchType = allGamePieces[x, y].matchValue;
            if (allGamePieces[x - 1, y].matchValue.Equals(matchType) && allGamePieces[x - 2, y].matchValue.Equals(matchType))
            {
                return true;
            }
        }
        if (y > 1)
        {
            MatchValue matchType = allGamePieces[x, y].matchValue;
            if (allGamePieces[x, y - 1].matchValue.Equals(matchType) && allGamePieces[x, y - 2].matchValue.Equals(matchType))
            {
                return true;
            }
        }
        return false;
    }
    
    List<GamePiece> GetRowPieces(int row)
    {
        List<GamePiece> gamePieces = new List<GamePiece>();

        for (int i = 0; i < width; i++)
        {
            if (allGamePieces[i, row] != null)
            {
                gamePieces.Add(allGamePieces[i, row]);
            }
        }
        return gamePieces;
    }

    List<GamePiece> GetColumnPieces(int column)
    {
        List<GamePiece> gamePieces = new List<GamePiece>();

        for (int i = 0; i < height; i++)
        {
            if (allGamePieces[column, i] != null)
            {
                gamePieces.Add(allGamePieces[column, i]);
            }
        }
        return gamePieces;
    }

    List<GamePiece> GetAdjacentPieces(int x, int y, int offset = 1)
    {
        List<GamePiece> gamePieces = new List<GamePiece>();

        for (int i = x - offset; i <= x + offset; i++)
        {
            for (int j = y - offset; j <= y + offset; j++)
            {
                if (IsInBorder(i, j))
                {
                    gamePieces.Add(allGamePieces[i, j]);
                }

            }
        }

        return gamePieces;
    }
    

    public List<GamePiece> GetAffectedPiecesByItems(List<GamePiece> gamePieces)
    {

        List<GamePiece> allPiecesToClear = new List<GamePiece>();

        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                List<GamePiece> piecesToClear = new List<GamePiece>();

                Item ıtem = piece.GetComponent<Item>();

                if (ıtem != null)
                {
                    switch (ıtem.ıtemType)
                    {
                        case ItemType.RocketColumn:
                            piecesToClear = GetColumnPieces(ıtem.xIndex);
                            break;
                        case ItemType.RocketRow:
                            piecesToClear = GetRowPieces(ıtem.yIndex);
                            break;
                        case ItemType.Bomb:
                            piecesToClear = GetAdjacentPieces(ıtem.xIndex, ıtem.yIndex, 1);
                            break;
                        case ItemType.Disco:

                            break;
                    }
                    allPiecesToClear = allPiecesToClear.Union(piecesToClear).ToList();
                }
            }
        }
        return allPiecesToClear;
    }

    
    
    // test if the Board is deadlocked
    public void TestDeadlock()
    {
        boardDeadlock.IsDeadlocked(allGamePieces, 3);
    }

    // invoke the ShuffleBoardRoutine (called by a button for testing)
    public void ShuffleBoard()
    {
        // only shuffle if the Board permits user input
        if (playerInputEnabled)
        {
            StartCoroutine(boardShuffler.ShuffleBoardRoutine(this));
        }
    }

}
