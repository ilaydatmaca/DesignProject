using UnityEngine;
using UnityEngine.UI;


public class MessageWindow : MonoBehaviour 
{
	public Image messageImage;
	public Text messageText;
	public Text buttonText;

    // sprite for losers
    public Sprite loseIcon;

    // sprite for winners
    public Sprite winIcon;

    // sprite for the level goal
    public Sprite goalIcon;
    

    void ShowMessage(Sprite sprite = null, string message = "", string buttonMsg = "start")
	{
		if (messageImage != null) 
		{
			messageImage.sprite = sprite;
		}

        if (messageText != null)
        {
            messageText.text = message;
        }
			
        if (buttonText != null)
        {
            buttonText.text = buttonMsg;
        }
	}
    
    public void ShowLoseMessage()
    {
        GetComponent<MovingScreen>().MoveOn();
        ShowMessage(loseIcon, "level\nfailed", "ok");
    }
    
    public void ShowWinMessage()
    {
        GetComponent<MovingScreen>().MoveOn();
        ShowMessage(winIcon, "level\ncomplete", "ok");
    }
    
    public void ShowScoreMessage(int scoreGoal)
    {
        string message = "score goal \n" + scoreGoal;
        ShowMessage(goalIcon, message, "start");
    }
}
