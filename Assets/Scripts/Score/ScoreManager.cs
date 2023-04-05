using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager> 
{
	public int currentScorePlayer1;
	public int currentScorePlayer2;
	
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
			player1ScoreText.text = currentScorePlayer1.ToString ();
		}
		
		if (player2ScoreText != null) 
		{
			player2ScoreText.text = currentScorePlayer2.ToString ();
		}
	}

	public void AddScore(int value)
	{
		if (_roundManager.turnView == _roundManager.player1View)
		{
			currentScorePlayer1 += value;
		}
		else if (_roundManager.turnView == _roundManager.player2View)
		{
			currentScorePlayer2 += value;
		}
		UpdateScoreText();

	}

}
