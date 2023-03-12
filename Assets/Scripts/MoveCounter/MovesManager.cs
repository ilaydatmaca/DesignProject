
using UnityEngine;
using UnityEngine.UI;

public class MovesManager : Singleton<MovesManager>
{
    public GameObject movesCounterArea;
    public Text movesLeftText;
    
    public int movesLeft;


    private void Start()
    {
        EnableMovesCounterArea();
        UpdateMovesText();
    }
    
    
    void EnableMovesCounterArea()
    {
        if (movesCounterArea != null)
        {
            movesCounterArea.SetActive(true);
        }
    }
    
    public void DecreaseMoveLeft()
    {
        movesLeft--;

        UpdateMovesText();
    }


    void UpdateMovesText()
    {
        if (movesLeftText != null)
        {
            movesLeftText.text = movesLeft.ToString();
        }
    }
}
