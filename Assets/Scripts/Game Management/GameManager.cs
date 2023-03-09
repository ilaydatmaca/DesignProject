using UnityEngine;

//using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

// the GameManager is the master controller for the GamePlay

[RequireComponent(typeof(LevelGoal))]
public class GameManager : Singleton<GameManager>
{
    Board _board;
    
    bool _isPlayerReady = false;

    bool _isGameOver = false;
    public bool IsGameOver { get { return _isGameOver; } set { _isGameOver = value; } }

    bool _isWinner;

    // are we ready to load/reload a new level?
    bool _isReadyToReload = false;
    
    LevelGoal _levelGoal;

    LevelGoalCollected _levelGoalCollected;

    // public reference to LevelGoalTimed component
    public LevelGoal LevelGoal { get { return _levelGoal; } }


    public override void Awake()
    {
        base.Awake();

        _levelGoal = GetComponent<LevelGoal>();
        _levelGoalCollected = GetComponent<LevelGoalCollected>();
        _board = Object.FindObjectOfType<Board>().GetComponent<Board>();

    }
    
    void Start()
    {
        if (UIManager.Instance != null)
        {
            // position ScoreStar horizontally
            if (UIManager.Instance.scoreMeter != null)
            {
                UIManager.Instance.scoreMeter.SetupStars(_levelGoal);
            }

            // use the Scene name as the Level name
            if (UIManager.Instance.levelNameText != null)
            {
                // get a reference to the current Scene
                Scene scene = SceneManager.GetActiveScene();
                UIManager.Instance.levelNameText.text = scene.name;
            }

            bool useTimer = (_levelGoal.levelCounter == LevelCounter.Timer);

            UIManager.Instance.EnableTimer(useTimer);
            UIManager.Instance.EnableMovesCounter(!useTimer);
        }

        // update the moves left UI
        _levelGoal.movesLeft++;
        UpdateMoves();

        // start the main game loop
        StartCoroutine("ExecuteGameLoop");
    }

    // update the Text component that shows our moves left
    public void UpdateMoves()
    {
        // if the LevelGoal is not timed (e.g. LevelGoalScored)...
        if (_levelGoal.levelCounter == LevelCounter.Moves)
        {
            // decrement a move
            _levelGoal.movesLeft--;

            // update the UI
            if (UIManager.Instance != null && UIManager.Instance.movesLeftText != null)
            {
                UIManager.Instance.movesLeftText.text = _levelGoal.movesLeft.ToString();
            }
        }
    }

    // this is the main coroutine for the Game, that determines are basic beginning/middle/end

    // each stage of the game must complete before we advance to the next stage
    // add as many stages here as necessary

    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine("StartGameRoutine");
        yield return StartCoroutine("PlayGameRoutine");

        // wait for board to refill
        yield return StartCoroutine("WaitForBoardRoutine", 0.5f);

        yield return StartCoroutine("EndGameRoutine");
    }

    // switches ready to begin status to true
    public void BeginGame()
    {
        _isPlayerReady = true;

    }

    // coroutine for the level introduction
    IEnumerator StartGameRoutine()
    {
        if (UIManager.Instance != null)
        {
            // show the message window with the level goal
            if (UIManager.Instance.messageWindow != null)
            {
                UIManager.Instance.messageWindow.GetComponent<RectXformMover>().MoveOn();
                int maxGoal = _levelGoal.scoreGoals.Length - 1;
                UIManager.Instance.messageWindow.ShowScoreMessage(_levelGoal.scoreGoals[maxGoal]);

                if (_levelGoal.levelCounter == LevelCounter.Timer)
                {
                    UIManager.Instance.messageWindow.ShowTimedGoal(_levelGoal.timeLeft);
                }
                else
                {
                    UIManager.Instance.messageWindow.ShowMovesGoal(_levelGoal.movesLeft);
                }
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

    // coroutine for game play
    IEnumerator PlayGameRoutine()
    {
        // if level is timed, start the timer
        if (_levelGoal.levelCounter == LevelCounter.Timer)
        {
            _levelGoal.StartCountdown();
        }
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
        if (_levelGoal.levelCounter == LevelCounter.Timer && UIManager.Instance != null
            && UIManager.Instance.timer != null)
        {
            UIManager.Instance.timer.FadeOff();
            UIManager.Instance.timer.paused = true;
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


        // if player beat the level goals, show the win screen and play the win sound
        if (_isWinner)
        {
            ShowWinScreen();
        } 
        // otherwise, show the lose screen and play the lose sound
		else
        {   
            ShowLoseScreen();
        }

        // wait one second
        yield return new WaitForSeconds(1f);

        // fade the screen 
        if (UIManager.Instance != null && UIManager.Instance.screenFader != null)
        {
            UIManager.Instance.screenFader.FadeOn();
        }  

        // wait until read to reload
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
            UIManager.Instance.messageWindow.GetComponent<RectXformMover>().MoveOn();
            UIManager.Instance.messageWindow.ShowWinMessage();
            UIManager.Instance.messageWindow.ShowCollectionGoal(false);

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
            UIManager.Instance.messageWindow.GetComponent<RectXformMover>().MoveOn();
            UIManager.Instance.messageWindow.ShowLoseMessage();
            UIManager.Instance.messageWindow.ShowCollectionGoal(false);

            string caption = "";
            if (_levelGoal.levelCounter == LevelCounter.Timer)
            {
                caption = "Out of time!";
            }
            else
            {
                caption = "Out of moves!";
            }

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

    // use this to acknowledge that the player is ready to reload
    public void ReloadScene()
    {
        _isReadyToReload = true;
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

                // update the scoreStars in the Level Goal component
                _levelGoal.UpdateScoreStars(ScoreManager.Instance.CurrentScore);

                if (UIManager.Instance != null && UIManager.Instance.scoreMeter != null)
                {
                    UIManager.Instance.scoreMeter.UpdateScoreMeter(ScoreManager.Instance.CurrentScore, 
                        _levelGoal.scoreStars);
                }
            }

            // play scoring sound clip
            if (SoundManager.Instance != null && piece.clearSound != null)
            {
                SoundManager.Instance.PlayClipAtPoint(piece.clearSound, Vector3.zero, SoundManager.Instance.fxVolume);
            }
        }
    }

    public void AddTime(int timeValue)
    {
        if (_levelGoal.levelCounter == LevelCounter.Timer)
        {
            _levelGoal.AddTime(timeValue);
        }
    }


}
