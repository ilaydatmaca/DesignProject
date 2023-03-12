

public class UIManager : Singleton<UIManager>
{
    public ScreenFader screenFader;

    public MessageWindow messageWindow;
    
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


}
