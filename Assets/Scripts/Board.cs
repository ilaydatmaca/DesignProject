using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Board : MonoBehaviour
{
    public int width = 8;
    public int height = 9;

    public int borderSize = 1;

    public GameObject cellPrefab;

    Cell[,] m_allTiles;
    public GamePiece[,] m_allGamePieces;
    
    public int m_scoreMultiplier = 0;

    private ParticleManager m_particleManager;
    private ItemFactory _itemFactory;

    void Awake()
    {
        _itemFactory = FindObjectOfType<ItemFactory>();
    }
    
    void Start()
    {
        m_allTiles = new Cell[width, height];
        m_allGamePieces = new GamePiece[width, height];
        m_particleManager = GameObject.FindWithTag("ParticleManager").GetComponent<ParticleManager>();
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
                GameObject cell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, transform);
                m_allTiles[x, y] = cell.GetComponent<Cell>();
                m_allTiles[x, y].Init(x, y);
            }
            
        }
    }

    void SetupCamera()
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
                if (m_allGamePieces[i, j] == null)
                {
                    MakeRandomGamePiece(i, j);
                    int iteration = 0;
                    
                    while (HasMatchOnFill(i, j) && iteration < maxIterations)
                    {
                        DestroyAt(i, j);
                        MakeRandomGamePiece(i, j);
                        iteration++;
                    }
                }
            }
        }
    }

    bool HasMatchOnFill(int x, int y)
    {
        if (x > 1)
        {
            MatchType matchType = m_allGamePieces[x, y].matchValue;
            if (m_allGamePieces[x - 1, y].matchValue.Equals(matchType) && m_allGamePieces[x - 2, y].matchValue.Equals(matchType))
            {
                return true;
            }
        }
        if (y > 1)
        {
            MatchType matchType = m_allGamePieces[x, y].matchValue;
            if (m_allGamePieces[x, y - 1].matchValue.Equals(matchType) && m_allGamePieces[x, y - 2].matchValue.Equals(matchType))
            {
                return true;
            }
        }
        return false;
    }
    
    void MakeRandomGamePiece(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            GameObject randomPiece = Instantiate(_itemFactory.GetRandomGamePiece(), Vector3.zero, Quaternion.identity, transform) as GameObject;
            randomPiece.GetComponent<GamePiece>().Init(this, x, y );
            m_allGamePieces[x, y] = randomPiece.GetComponent<GamePiece>();
            randomPiece.GetComponent<GamePiece>().Fall();
        }
    }

    GameObject MakeItem(ItemType itemType, int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            GameObject bomb = Instantiate(_itemFactory.GetItem(itemType), new Vector3(x, y, 0), Quaternion.identity, transform) as GameObject;
            bomb.GetComponent<Item>().Init(this, x, y);
            return bomb;
        }
        return null;
    }

    public bool IsWithinBounds(int x, int y)
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

    
    void DestroyAt(int x, int y)
    {
        GamePiece pieceToClear = m_allGamePieces[x, y];

        if (pieceToClear != null)
        {
            m_allGamePieces[x, y] = null;
            Destroy(pieceToClear.gameObject);
        }
    }

    public void ClearPieceAt(List<GamePiece> gamePieces, List<GamePiece> bombedPieces)
    {
        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                DestroyAt(piece.xIndex, piece.yIndex);
                int bonus = 0;

                if (gamePieces.Count >= 4)
                {
                    bonus = 20;
                }
                
                piece.ScorePoints(m_scoreMultiplier);

                if (m_particleManager != null)
                {
                    if (bombedPieces.Contains(piece))
                    {
                        m_particleManager.BombFXAt(piece.xIndex, piece.yIndex);
                    }
                    else
                    {
                        m_particleManager.ClearPieceFXAt(piece.xIndex, piece.yIndex);
                    }
                }
            }
        }
    }
    
    public GameObject CheckItem(int x, int y, Vector2 swapDirection, List<GamePiece> gamePieces)
    {
        GameObject bomb = null;

        if (gamePieces.Count == 4)
        {
            if (swapDirection.x != 0)
            {
                bomb = MakeItem(ItemType.RocketRow, x, y);
            }
            else
            {
                bomb = MakeItem(ItemType.RocketColumn, x, y);
            }
        }
        else if (gamePieces.Count == 5)
        {
            bomb = MakeItem(ItemType.Bomb, x, y);

        }
        else if (gamePieces.Count == 6)
        { 
            bomb = MakeItem(ItemType.Disco, x, y);
        }
        return bomb;
    }
    
    public List<GamePiece> GetBombedPieces(List<GamePiece> gamePieces)
    {
        List<GamePiece> allPiecesToClear = new List<GamePiece>();

        foreach (GamePiece piece in gamePieces)
        {
            if (piece != null)
            {
                List<GamePiece> piecesToClear = new List<GamePiece>();

                Item item = piece.GetComponent<Item>();

                if (item != null)
                {
                    switch (item._itemType)
                    {
                        case ItemType.RocketColumn:
                            piecesToClear = GetColumnPieces(item.xIndex);
                            break;
                        case ItemType.RocketRow:
                            piecesToClear = GetRowPieces(item.yIndex);
                            break;
                        case ItemType.Bomb:
                            piecesToClear = GetAdjacentPieces(item.xIndex, item.yIndex, 1);
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
    List<GamePiece> GetRowPieces(int row)
    {
        List<GamePiece> gamePieces = new List<GamePiece>();

        for (int i = 0; i < width; i++)
        {
            if (m_allGamePieces[i, row] != null)
            {
                gamePieces.Add(m_allGamePieces[i, row]);
            }
        }
        return gamePieces;
    }

    List<GamePiece> GetColumnPieces(int column)
    {
        List<GamePiece> gamePieces = new List<GamePiece>();

        for (int i = 0; i < height; i++)
        {
            if (m_allGamePieces[column, i] != null)
            {
                gamePieces.Add(m_allGamePieces[column, i]);
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
                if (IsWithinBounds(i, j))
                {
                    gamePieces.Add(m_allGamePieces[i, j]);
                }
            }
        }
        return gamePieces;
    }

}
