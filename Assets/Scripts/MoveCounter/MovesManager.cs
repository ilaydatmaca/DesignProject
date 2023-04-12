using Photon.Pun;
using TMPro;

public class MovesManager : Singleton<MovesManager>
{
    public bool noMoreMoves;
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
    
    void UpdateMovesText()
    {
        if (moveText != null)
        {
            moveText.text = _moveLeft + " moves left";
        }
    }

    void SetStateMoveLeft()
    {
        moveText.enabled = RoundManager.Instance.turnView.IsMine;
    }
    
}
