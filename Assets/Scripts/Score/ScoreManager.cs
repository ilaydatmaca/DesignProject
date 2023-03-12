using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager> 
{
	int _currentScore;
	public int CurrentScore { get => _currentScore; }

	public Text scoreText;
	public int maxScore = 500;

	void Start () 
	{
		UpdateScoreText ();
	}

	void UpdateScoreText()
	{
		if (scoreText != null) 
		{
			scoreText.text = _currentScore.ToString ();
		}
	}

	public void AddScore(int value)
	{
		_currentScore += value;
		UpdateScoreText ();
	}

}
