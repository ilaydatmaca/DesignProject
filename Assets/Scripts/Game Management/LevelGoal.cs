using System;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LevelGoal : MonoBehaviour
{
    private static PlayFabLogin playfabLogin;
    
    private void Awake()
    {
        playfabLogin = FindObjectOfType<PlayFabLogin>().GetComponent<PlayFabLogin>();
        Console.WriteLine();
    }

    public bool IsGameOver()
    {
        if (RoundManager.Instance.roundComplete)
        {
            int score = ScoreManager.Instance.GetMyScore();
            playfabLogin.SendLeaderboard(score);
            return true;
        }
        return false;
    }

    public GameState IsWinner()
    {
        GameState state = GameState.Lose;
        if (ScoreManager.Instance.currentScorePlayer1 > ScoreManager.Instance.currentScorePlayer2)
        {
            if (RoundManager.Instance.player1View.IsMine)
            {
                state = GameState.Win;
            }
        }
        else if (ScoreManager.Instance.currentScorePlayer2 > ScoreManager.Instance.currentScorePlayer1)
        {
            if (RoundManager.Instance.player2View.IsMine)
            {
                state = GameState.Win;
            }
        }
        else
        {
            state = GameState.Draw;
        }
        return state;
    }
    
}
