using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int movesLeft = 30;

    public int scoreGoal = 10000;

    public ScreenFader screenFader;
    Board m_board;

    public TextMeshProUGUI movesLeftText;

    bool isReadyToBegin = false;
    bool isGameOver = false;
    bool isWinner = false;
    bool isReadyToReload = false;


    public MessageWindow messageWindow;
    public Sprite loseIcon;
    public Sprite winIcon;
    public Sprite goalIcon;

    void Start()
    {
        m_board = GameObject.FindObjectOfType<Board>().GetComponent<Board>();
        UpdateMoves();
        StartCoroutine("ExecuteGameLoop");
    }

    public void UpdateMoves()
    {
        if (movesLeftText != null)
        {
            movesLeftText.text = movesLeft.ToString();
        }
    }

    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine("StartGameRoutine");
        yield return StartCoroutine("PlayGameRoutine");
        yield return StartCoroutine("EndGameRoutine");

    }

    public void BeginGame()
    {
        isReadyToBegin = true;
    }

    IEnumerator StartGameRoutine()
    {
        if (messageWindow != null)
        {
            messageWindow.GetComponent<RectXformMover>().MoveOn();
            messageWindow.ShowMessage(goalIcon, "score goal\n" + scoreGoal.ToString(), "start");
        }
        while (!isReadyToBegin)
        {
            yield return null;
        }

        if (screenFader != null)
        {
            screenFader.FadeOff();
        }

        yield return new WaitForSeconds(0.5f);

        if (m_board != null)
        {
            m_board.SetupBoard();
        }
    }

    IEnumerator PlayGameRoutine()
    {
        while (!isGameOver)
        {
            if (movesLeft == 0)
            {
                isGameOver = true;
                isWinner = false;
            }
            yield return null;
        }    
    }

    IEnumerator EndGameRoutine()
    {
        isReadyToReload = false;
        if (screenFader != null)
        {
            screenFader.FadeOn();
        }
        if (isWinner)
        {
            if (messageWindow != null)
            {
                messageWindow.GetComponent<RectXformMover>().MoveOn();
                messageWindow.ShowMessage(winIcon, "You Win", "OK");
            }
        }
        else
        {
            if (messageWindow != null)
            {
                messageWindow.GetComponent<RectXformMover>().MoveOn();
                messageWindow.ShowMessage(loseIcon, "You Lose", "OK");
            }

        }
        while (!isReadyToReload)
        {
            yield return null;
        }

        SceneManager.LoadScene("Scenes/SampleScene");

    }

    public void ReloadScene()
    {
        isReadyToReload = true;
    }

   
}
