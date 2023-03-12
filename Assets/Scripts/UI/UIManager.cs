

public class UIManager : Singleton<UIManager>
{
    public ScreenFader screenFader;


    public ScoreMeter scoreMeter;

    public MessageWindow messageWindow;


    public Timer timer;

    public override void Awake()
    {

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

    public void EnableTimer(bool state)
    {
        if (timer != null)
        {
            timer.gameObject.SetActive(state);
        }
    }


}
