using UnityEngine;
using Random = UnityEngine.Random;


public class ItemFactory : MonoBehaviour
{
    public GameObject[] bombPrefab;
    public GameObject[] rocketColumnPrefab;
    public GameObject[] rocketRowPrefab;
    public GameObject discoPrefab;
    
    public GameObject[] cellPrefabs;

    private Board _board;
    private ItemManager _itemManager;
    private ClearManager _clearManager;

    private void Awake()
    {
        _board = GetComponent<Board>();
        _itemManager = GetComponent<ItemManager>();
        _clearManager = FindObjectOfType<Board>().GetComponent<ClearManager>();
    }

    GameObject GetRandomGamePiece()
    {
        int randomIdx = Random.Range(0, cellPrefabs.Length);
        if (cellPrefabs[randomIdx] == null)
        {
            Debug.LogWarning("ERROR:  BOARD.GetRandomObject at index " + randomIdx + "does not contain a valid GameObject!");
        }
        return cellPrefabs[randomIdx];
    }

    GameObject GetItem(ItemType itemType, MatchValue matchValue)
    {
        GameObject prefab = null;

        switch (itemType)
        {
            case ItemType.Bomb:
                prefab = FindGamePieceByMatchValue(bombPrefab, matchValue);
                break;
            case ItemType.RocketColumn:
                prefab = FindGamePieceByMatchValue(rocketColumnPrefab, matchValue);
                break;
            case ItemType.RocketRow:
                prefab = FindGamePieceByMatchValue(rocketRowPrefab, matchValue);
                break;
            case ItemType.Disco:
                prefab = discoPrefab;
                break;
        }

        return prefab;
    }
    
    GameObject FindGamePieceByMatchValue(GameObject[] prefabs, MatchValue matchValue)
    {
        if (matchValue == MatchValue.None)
        {
            return null;
        }

        foreach (GameObject prefab in prefabs)
        {
            GamePiece piece = prefab.GetComponent<GamePiece>();

            if (piece != null)
            {
                if (piece.matchValue == matchValue)
                {
                    return prefab;
                }
            }
        }

        return null;
    }
    
    

    public void MakeRandomGamePiece(int x, int y)
    {
        if (_board.IsInBorder(x, y))
        {
            GameObject randomPiece = Instantiate(GetRandomGamePiece(), Vector3.zero, Quaternion.identity, transform);
            randomPiece.GetComponent<GamePiece>().Init(_board, x, y );
            _board.AllGamePieces[x, y] = randomPiece.GetComponent<GamePiece>();
            randomPiece.GetComponent<GamePiece>().Fall();
        }
    }
    
    public GameObject MakeItem(ItemType ıtemType, int x, int y, MatchValue matchValue = MatchValue.Wild)
    {
        if (_board.IsInBorder(x, y))
        {
            GameObject prefab = GetItem(ıtemType, matchValue);
            GameObject bomb = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity, transform);
            bomb.GetComponent<Item>().Init(_board, x, y);
            return bomb;
        }

        return null;
    }
    
    
    public void MakeColorBombBooster(int x, int y)
    {
        if (_board.IsInBorder(x, y))
        {
            GamePiece pieceToReplace = _board.AllGamePieces[x, y];

            if (pieceToReplace != null)
            {
                _clearManager.DestroyAt(x, y);
                GameObject bombObject = MakeItem(ItemType.Disco, x, y);
                _itemManager.InitItem(bombObject);
            }
        }
    }
    
    
}
