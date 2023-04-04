

public class UIManager : Singleton<UIManager>
{

    public MessageWindow messageWindow;
    
    public override void Awake()
    {

        base.Awake();

        if (messageWindow != null)
        {
            messageWindow.gameObject.SetActive(true);
        }
    }
    
}
