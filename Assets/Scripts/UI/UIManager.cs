
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public ScreenFader screenFader;

    public Text movesLeftText;

    public ScoreMeter scoreMeter;

    public MessageWindow messageWindow;

    public GameObject movesCounter;

    public Timer timer;

    public LevelGoal levelGoal;
    public override void Awake()
    {
        levelGoal = FindObjectOfType<GameManager>().GetComponent<LevelGoal>();

        base.Awake();

        if (messageWindow != null)
        {
            messageWindow.gameObject.SetActive(true);
        }

        if (screenFader != null)
        {
            screenFader.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        UpdateMovesText();
    }

    public void EnableTimer(bool state)
    {
        if (timer != null)
        {
            timer.gameObject.SetActive(state);
        }
    }

    public void EnableMovesCounter(bool state)
    {
        if (movesCounter != null)
        {
            movesCounter.SetActive(state);
        }
    }

    public void UpdateMovesText()
    {
        if (movesLeftText != null)
        {
            movesLeftText.text = levelGoal.movesLeft.ToString();
        }
    }


}
