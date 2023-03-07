
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    
    public GameObject bombPrefab;
    public GameObject rocketColumnPrefab;
    public GameObject rocketRowPrefab;
    public GameObject discoPrefab;
    
    public GameObject[] cellPrefabs;
    
    public GameObject GetRandomGamePiece()
    {
        int randomIdx = Random.Range(0, cellPrefabs.Length);
        if (cellPrefabs[randomIdx] == null)
        {
            Debug.LogWarning("ERROR:  BOARD.GetRandomObject at index " + randomIdx + "does not contain a valid GameObject!");
        }
        return cellPrefabs[randomIdx];
    }

    public GameObject GetItem(ItemType itemType)
    {
        GameObject prefab = null;

        switch (itemType)
        {
            case ItemType.Bomb:
                prefab = bombPrefab;
                break;
            case ItemType.RocketColumn:
                prefab = rocketColumnPrefab;
                break;
            case ItemType.RocketRow:
                prefab = rocketRowPrefab;
                break;
            case ItemType.Disco:
                prefab = discoPrefab;
                break;
        }

        return prefab;
    }

}
