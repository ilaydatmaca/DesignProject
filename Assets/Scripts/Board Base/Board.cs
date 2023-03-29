using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;

    public int borderSize;

    public GameObject cellPrefab;

    [HideInInspector] public GameObject clickedCellItem;
    [HideInInspector] public GameObject targetCellItem;
    
    [HideInInspector] public Cell clickedCell;
    [HideInInspector] public Cell targetCell;
    
    Cell[,] _allTiles;
    public GamePiece[,] AllGamePieces;
    
    public bool playerInputEnabled = true;
    public bool isRefilling;
    
    public float swapTime = 0.5f;
    public int scoreMultiplier;
    public float delay = 0.2f;


    private ItemFactory _itemFactory;
    private ShuffleManager _shuffleManager;
    private ClearManager _clearManager;
    
    private void Awake()
    {
        _shuffleManager = GetComponent<ShuffleManager>();
        _itemFactory = GetComponent<ItemFactory>();
        _clearManager = GetComponent<ClearManager>();
    }

    void Start()
    {
        _allTiles = new Cell[width, height];
        AllGamePieces = new GamePiece[width, height];
    }
    
    public void SetupBoard()
    {
        SetupCells();
        SetupCamera();

        FillBoard();
    }
    
    void SetupCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_allTiles[x, y] == null)
                {
                    GameObject tile = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, transform);
                    _allTiles[x, y] = tile.GetComponent<Cell>();
                    _allTiles[x, y].Init(x, y);
                }
            }
        }
    }
    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3((float)(width - 1) / 2f, ((float)(height - 1) / 2f) +2.5f, -10f);

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
                if (AllGamePieces[i, j] == null)
                {
                    _itemFactory.MakeRandomGamePiece(i, j);
                    int iteration = 0;

                    while (HasMatchOnFill(i, j) && iteration < maxIterations)
                    {
                        _clearManager.DestroyAt(i, j);
                        _itemFactory.MakeRandomGamePiece(i, j);

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
        return false;    
    }
    
    public bool HasMatchOnFill(int x, int y)
    {
        if (x > 1)
        {
            MatchValue matchType = AllGamePieces[x, y].matchValue;
            if (AllGamePieces[x - 1, y].matchValue.Equals(matchType) && AllGamePieces[x - 2, y].matchValue.Equals(matchType))
            {
                return true;
            }
        }
        if (y > 1)
        {
            MatchValue matchType = AllGamePieces[x, y].matchValue;
            if (AllGamePieces[x, y - 1].matchValue.Equals(matchType) && AllGamePieces[x, y - 2].matchValue.Equals(matchType))
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
            if (AllGamePieces[i, row] != null)
            {
                gamePieces.Add(AllGamePieces[i, row]);
            }
        }
        return gamePieces;
    }

    List<GamePiece> GetColumnPieces(int column)
    {
        List<GamePiece> gamePieces = new List<GamePiece>();

        for (int i = 0; i < height; i++)
        {
            if (AllGamePieces[column, i] != null)
            {
                gamePieces.Add(AllGamePieces[column, i]);
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
                    gamePieces.Add(AllGamePieces[i, j]);
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

}
