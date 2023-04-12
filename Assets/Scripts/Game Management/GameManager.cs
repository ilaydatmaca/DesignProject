using UnityEngine;
using System.Collections;
using Photon.Pun;

public class GameManager : Singleton<GameManager>
{
    private bool _isPlayerReady;
    private bool _isGameOver;
    private bool _isReadyToReload;

    private GameState _gameState;

    public bool IsGameOver;

    private LevelGoal _levelGoal;
    private Board _board;

    public bool paused = true;

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
        yield return new WaitForSeconds(RoundManager.Instance.waitTimeForRounds);

    }
    
    IEnumerator PlayGameRoutine()
    {
        paused = false;
        RoundManager.Instance.InitRound();
        MovesManager.Instance.Init();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonTimer.SetStartTime();
        }

        while (!_isGameOver)
        {
            _isGameOver = _levelGoal.IsGameOver();
            _gameState = _levelGoal.IsWinner();

            yield return null;
        }
    }
    
    IEnumerator WaitForBoardRoutine(float delay = 0f)
    {
        paused = true;

        if (_board != null)
        {
            yield return new WaitForSeconds(_board.swapTime);
            
            while (_board.isRefilling)
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(delay);
    }
    
    
    IEnumerator EndGameRoutine()
    {
        _isReadyToReload = false;

        if (_gameState == GameState.Win)
        {
            ShowWinScreen();
        } 
        else if (_gameState == GameState.Lose)
        {   
            ShowLoseScreen();
        }
        else
        {
            ShowDrawScreen();
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
        //PhotonTimer.Add;
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

    void ShowDrawScreen()
    {
        PopupWindow.Instance.ShowDrawWindow();
        SoundManager.Instance.PlayLoseSound();
    }

    public void ScorePoints(GamePiece piece, int multiplier = 1, int bonus = 0)
    {
        if (piece != null)
        {
            if (ScoreManager.Instance != null)
            {
                int addingScore = piece.scoreValue * multiplier + bonus;
                ScoreManager.Instance.AddScore(addingScore);
            }

            if (SoundManager.Instance != null && piece.clearSound != null)
            {
                SoundManager.Instance.PlayClipAtPoint(piece.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
            }
        }
    }


}
