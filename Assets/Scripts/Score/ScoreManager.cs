using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager> 
{
	private int _currentScorePlayer1;
	private int _currentScorePlayer2;
	
	public Text player1ScoreText;
	public Text player2ScoreText;

	private RoundManager _roundManager;
	public override void Awake()
	{
		base.Awake();
		_roundManager = FindObjectOfType<GameManager>().GetComponent<RoundManager>();
	}

	void Start() 
	{
		UpdateScoreText();
	}

	void UpdateScoreText()
	{
		if (player1ScoreText != null) 
		{
			player1ScoreText.text = _currentScorePlayer1.ToString ();
		}
		
		if (player2ScoreText != null) 
		{
			player2ScoreText.text = _currentScorePlayer2.ToString ();
		}
	}

	public void AddScore(int value)
	{
		if (_roundManager.turnView == _roundManager.player1View)
		{
			_currentScorePlayer1 += value;
		}
		else if (_roundManager.turnView == _roundManager.player2View)
		{
			_currentScorePlayer2 += value;
		}
		UpdateScoreText();

	}

}
