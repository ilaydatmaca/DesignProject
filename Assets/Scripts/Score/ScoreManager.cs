using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager> 
{
	public int currentScorePlayer1;
	public int currentScorePlayer2;
	
	public Text player1ScoreText;
	public Text player2ScoreText;

	void Start() 
	{
		UpdateScoreText();
	}

	void UpdateScoreText()
	{
		if (player1ScoreText != null) 
		{
			player1ScoreText.text = currentScorePlayer1.ToString ();
		}
		
		if (player2ScoreText != null) 
		{
			player2ScoreText.text = currentScorePlayer2.ToString ();
		}
	}

	public void AddScore(int value)
	{
		if (RoundManager.Instance.turnView == RoundManager.Instance.player1View)
		{
			currentScorePlayer1 += value;
		}
		else if (RoundManager.Instance.turnView == RoundManager.Instance.player2View)
		{
			currentScorePlayer2 += value;
		}
		UpdateScoreText();

	}

	public int GetMyScore()
	{
		int myScore = 0;
		if (RoundManager.Instance.player1View.IsMine)
		{
			myScore = currentScorePlayer1;
		}
		else if (RoundManager.Instance.player2View.IsMine)
		{
			myScore = currentScorePlayer2;
		}

		return myScore;
	}

}
