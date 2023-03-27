using System.Collections.Generic;
using UnityEngine;

public enum Direction{
    None,
    Up,
    Down,
    Left,
    Right
    
}
public class Directions : MonoBehaviour
{

    Vector2 GetVectorFromDirection(Direction direction)
    {
        Vector2 returnDirection = new Vector2(0, 0);
        
        switch (direction)
        {
            case Direction.None:
                returnDirection = new Vector2(0, 0);
                break;
            case Direction.Up:
                returnDirection = new Vector2(0, 1);
                break;
            case Direction.Down:
                returnDirection = new Vector2(0, -1);
                break;
            case Direction.Left:
                returnDirection = new Vector2(-1, 0);
                break;
            case Direction.Right:
                returnDirection = new Vector2(1, 0);
                break;
        }

        return returnDirection;
    }


    public List<Vector2> GetVectorsFromList(List<Direction> list)
    {
        List<Vector2> vectors = new List<Vector2>();

        foreach (Direction direction in list)
        {
            vectors.Add(GetVectorFromDirection(direction));
        }

        return vectors;
    }
}
