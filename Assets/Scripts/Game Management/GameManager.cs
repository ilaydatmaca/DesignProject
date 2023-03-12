using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    Board _board;
    
    bool _isPlayerReady;
    bool _isGameOver;
    bool _isWinner;
    bool _isReadyToReload;

    public bool IsGameOver { get => _isGameOver; }

    LevelGoal _levelGoal;


    public override void Awake()
    {
        base.Awake();

        _levelGoal = GetComponent<LevelGoal>();
        _board = FindObjectOfType<Board>().GetComponent<Board>();

    }
    
    public void BeginGame()
    {
        _isPlayerReady = true;
        _isReadyToReload = true;

    }
    void Start()
    { 
        StartCoroutine(ExecuteGameLoop());
    }

    public void UpdateMoves()
    {
        MovesManager.Instance.DecreaseMoveLeft();
    }

    public void AddTime(int timeValue)
    {
        TimeManager.Instance.AddTime(timeValue);
    }


    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine(StartGameRoutine());
        yield return StartCoroutine(PlayGameRoutine());

        // wait for board to refill
        yield return StartCoroutine(WaitForBoardRoutine(0.5f));

        yield return StartCoroutine(EndGameRoutine());
    }
    

    IEnumerator StartGameRoutine()
    {
        if (UIManager.Instance != null)
        {
            // show the message window with the level goal
            if (UIManager.Instance.messageWindow != null)
            {
                UIManager.Instance.messageWindow.GetComponent<MovingScreen>().MoveOn();
                UIManager.Instance.messageWindow.ShowScoreMessage(ScoreManager.Instance.maxScore);

                    UIManager.Instance.messageWindow.ShowTimedGoal(TimeManager.Instance.currentTime);
                    UIManager.Instance.messageWindow.ShowMovesGoal(MovesManager.Instance.movesLeft);
            }
        }

        // wait until the player is ready
        while (!_isPlayerReady)
        {
            yield return null;
        }

        // fade off the ScreenFader
        if (UIManager.Instance != null && UIManager.Instance.screenFader != null)
        {
            UIManager.Instance.screenFader.FadeOff();
        }

        // wait half a second
        yield return new WaitForSeconds(0.5f);

        // setup the Board
        if (_board != null)
        {
            _board.SetupBoard();
        }
    }

    
    IEnumerator PlayGameRoutine()
    {
        
       TimeManager.Instance.StartCountdown();
        
        // while the end game condition is not true, we keep playing
        // just keep waiting one frame and checking for game conditions
        while (!_isGameOver)
        {
            _isGameOver = _levelGoal.IsGameOver();
            _isWinner = _levelGoal.IsWinner();

            // wait one frame
            yield return null;
        }
    }

    IEnumerator WaitForBoardRoutine(float delay = 0f)
    {
        if (TimeManager.Instance != null && TimeManager.Instance.timer != null)
        {
            TimeManager.Instance.timer.FadeOff();
            TimeManager.Instance.timer.paused = true;
        }

        if (_board != null)
        {
            // this accounts for the swapTime delay in the Board's SwitchTilesRoutine BEFORE ClearAndRefillRoutine is invoked
            yield return new WaitForSeconds(_board.swapTime);

            // wait while the Board is refilling
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
        // set ready to reload to false to give the player time to read the screen
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

        if (UIManager.Instance != null && UIManager.Instance.screenFader != null)
        {
            UIManager.Instance.screenFader.FadeOn();
        }  

        while (!_isReadyToReload)
        {
            yield return null;
        }

        // reload the scene (you would customize this to go back to the menu or go to the next level
        // but we just reload the same scene in this demo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		
    }

    void ShowWinScreen()
    {
        if (UIManager.Instance != null && UIManager.Instance.messageWindow != null)
        {
            UIManager.Instance.messageWindow.GetComponent<MovingScreen>().MoveOn();
            UIManager.Instance.messageWindow.ShowWinMessage();

            if (ScoreManager.Instance != null)
            {
                string scoreStr = "you scored\n" + ScoreManager.Instance.CurrentScore.ToString() + " points!";
                UIManager.Instance.messageWindow.ShowGoalCaption(scoreStr,0,70);
            }

            if (UIManager.Instance.messageWindow.goalCompleteIcon != null)
            {
                UIManager.Instance.messageWindow.ShowGoalImage(UIManager.Instance.messageWindow.goalCompleteIcon);
            }
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWinSound();
        }
    }

    void ShowLoseScreen()
    {
        if (UIManager.Instance != null && UIManager.Instance.messageWindow != null)
        {
            UIManager.Instance.messageWindow.GetComponent<MovingScreen>().MoveOn();
            UIManager.Instance.messageWindow.ShowLoseMessage();

            string caption = "";
            caption = "Out of time!";

            UIManager.Instance.messageWindow.ShowGoalCaption(caption, 0, 70);

            if (UIManager.Instance.messageWindow.goalFailedIcon != null)
            {
                UIManager.Instance.messageWindow.ShowGoalImage(UIManager.Instance.messageWindow.goalFailedIcon);
            }

        }
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayLoseSound();
        }
    }

    // score points and play a sound
    public void ScorePoints(GamePiece piece, int multiplier = 1, int bonus = 0)
    {
        if (piece != null)
        {
            if (ScoreManager.Instance != null)
            {
                // score points
                ScoreManager.Instance.AddScore(piece.scoreValue * multiplier + bonus);

                if (UIManager.Instance != null && UIManager.Instance.scoreMeter != null)
                {
                    UIManager.Instance.scoreMeter.UpdateScoreMeter(ScoreManager.Instance.CurrentScore);
                }
            }

            // play scoring sound clip
            if (SoundManager.Instance != null && piece.clearSound != null)
            {
                SoundManager.Instance.PlayClipAtPoint(piece.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
            }
        }
    }


}
