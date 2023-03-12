
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

    private void Awake()
    {
        _board = GetComponent<Board>();
    }

    public GameObject GetRandomGamePiece()
    {
        int randomIdx = Random.Range(0, cellPrefabs.Length);
        if (cellPrefabs[randomIdx] == null)
        {
            Debug.LogWarning("ERROR:  BOARD.GetRandomObject at index " + randomIdx + "does not contain a valid GameObject!");
        }
        return cellPrefabs[randomIdx];
    }

    public GameObject GetItem(ItemType itemType, MatchValue matchValue)
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

        foreach (GameObject go in prefabs)
        {
            GamePiece piece = go.GetComponent<GamePiece>();

            if (piece != null)
            {
                if (piece.matchValue == matchValue)
                {
                    return go;
                }
            }
        }

        return null;
    }
    
    

    public void MakeRandomGamePiece(int x, int y)
    {
        if (_board.IsInBorder(x, y))
        {
            GameObject randomPiece = Instantiate(GetRandomGamePiece(), Vector3.zero, Quaternion.identity, transform) as GameObject;
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
            GameObject bomb = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity, transform) as GameObject;
            bomb.GetComponent<Item>().Init(_board, x, y);
            return bomb;
        }

        return null;
    }
    
    
}
