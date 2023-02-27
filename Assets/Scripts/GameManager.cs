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

    IEnumerator StartGameRoutine()
    {
        while (!isReadyToBegin)
        {
            yield return null;
            yield return new WaitForSeconds(2f);
            isReadyToBegin = true;
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
        if (screenFader != null)
        {
            screenFader.FadeOn();
        }
        if (isWinner)
        {
            Debug.Log("You Win");
        }
        else
        {
            Debug.Log("You Lose");

        }
        yield return null;

    }

   
}
