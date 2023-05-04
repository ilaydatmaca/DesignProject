using TMPro;
using UnityEngine;

public class MovesManager : Singleton<MovesManager>
{
    [HideInInspector] public bool noMoreMoves;
    public int moveNumberPerRound = 2;
    
    private int _moveLeft;

    public TMP_Text moveText;
    
    public void Init()
    {
        noMoreMoves = false;
        _moveLeft = moveNumberPerRound;
        
        UpdateMovesText();
        SetStateMoveLeft();

    }
    public void DecreaseMoveLeft()
    {
        _moveLeft--;
        UpdateMovesText();

        if (_moveLeft == 0)
        {
            noMoreMoves = true;
        }
    }
    
    public void IncreaseMoveLeft()
    {
        _moveLeft++;
        UpdateMovesText();
    }
    
    void UpdateMovesText()
    {
        if (moveText != null)
        {
            moveText.text = _moveLeft + " MOVES LEFT";
        }
    }

    void SetStateMoveLeft()
    {
        moveText.enabled = RoundManager.Instance.turnView.IsMine && !RoundManager.Instance.roundComplete;
    }
    
}
