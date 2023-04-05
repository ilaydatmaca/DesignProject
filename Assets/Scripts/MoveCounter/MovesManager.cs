
using TMPro;

public class MovesManager : Singleton<MovesManager>
{
    public bool noMoreMoves = false;

    public int moveNumberPerRound = 2;
    private int _moveLeft;

    public TMP_Text moveText;

    private RoundManager _roundManager;

    public override void Awake()
    {
        base.Awake();
        _roundManager = FindObjectOfType<GameManager>().GetComponent<RoundManager>();
    }

    public void InitMoves()
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
        moveText.enabled = _roundManager.turnView.IsMine;
    }
    
}
