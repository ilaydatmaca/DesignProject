using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager> 
{
	private int _currentScore;
	public int CurrentScore { get => _currentScore; }
	public int maxScore = 500;

	public Text scoreText;

	public ScoreMeter scoreMeter;

	void Start() 
	{
		UpdateScoreText();
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
		scoreMeter.UpdateScoreMeter(_currentScore);
		UpdateScoreText();
	}

}
