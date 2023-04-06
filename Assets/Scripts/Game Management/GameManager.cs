using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    private bool _isPlayerReady;
    private bool _isGameOver;
    private bool _isWinner;
    private bool _isReadyToReload;

    public bool IsGameOver { get; }

    private LevelGoal _levelGoal;
    private Board _board;

    public override void Awake()
    {
        base.Awake();

        _levelGoal = GetComponent<LevelGoal>();
        _board = FindObjectOfType<Board>().GetComponent<Board>();
    }
    
    void Start()
    { 
        StartCoroutine(ExecuteGameLoop());
    }
    
    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine(StartGameRoutine());
        yield return StartCoroutine(PlayGameRoutine());

        
        
        yield return StartCoroutine(WaitForBoardRoutine(0.5f));

        yield return StartCoroutine(EndGameRoutine());
    }

    IEnumerator StartGameRoutine()
    {
        _isPlayerReady = true;
        _isReadyToReload = true;
        
        while (!_isPlayerReady)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        if (_board != null)
        {
            _board.SetupBoard();
        }
    }
    
    IEnumerator PlayGameRoutine()
    {
        RoundManager.Instance.InitRound();
        TimeManager.Instance.paused = false;
        
        while (!_isGameOver)
        {
            _isGameOver = _levelGoal.IsGameOver();
            _isWinner = _levelGoal.IsWinner();

            yield return null;
        }
    }
    
    IEnumerator WaitForBoardRoutine(float delay = 0f)
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.paused = true;
        }

        if (_board != null)
        {
            yield return new WaitForSeconds(_board.swapTime);
            
            while (_board.isRefilling)
            {
                yield return null;
            }
        }
        // extra delay before we go to the EndGameRoutine
        yield return new WaitForSeconds(delay);
    }
    
    // coroutine for the end of the level
    IEnumerator EndGameRoutine()
    {
        _isReadyToReload = false;

        if (_isWinner)
        {
            ShowWinScreen();
        } 
        else
        {   
            ShowLoseScreen();
        }

        yield return new WaitForSeconds(1f);

        while (!_isReadyToReload)
        {
            yield return null;
        }
    }


    public void UpdateMoves()
    {
        MovesManager.Instance.DecreaseMoveLeft();
    }

    public void AddTime(int timeValue)
    {
        TimeManager.Instance.AddTime(timeValue);
    }

    void ShowWinScreen()
    { 
        PopupWindow.Instance.ShowWinWindow();
        SoundManager.Instance.PlayWinSound();
    }

    void ShowLoseScreen()
    {
        PopupWindow.Instance.ShowLoseWindow();
        SoundManager.Instance.PlayLoseSound();
    }


    public void ScorePoints(GamePiece piece, int multiplier = 1, int bonus = 0)
    {
        if (piece != null)
        {
            if (ScoreManager.Instance != null)
            {
                // score points
                int addingScore = piece.scoreValue * multiplier + bonus;
                ScoreManager.Instance.AddScore(addingScore);
            }

            // play scoring sound clip
            if (SoundManager.Instance != null && piece.clearSound != null)
            {
                SoundManager.Instance.PlayClipAtPoint(piece.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
            }
        }
    }


}
