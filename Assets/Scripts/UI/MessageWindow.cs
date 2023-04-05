using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageWindow : Singleton<MessageWindow>
{
    
    public GameObject losingPopup;
    public GameObject winningPopup;
    
    public void ShowLoseWindow()
    {
        losingPopup.SetActive(true);

    }
    
    public void ShowWinWindow()
    {
        winningPopup.SetActive(true);

    }

    public void Restart()
    {
        losingPopup.SetActive(false);
        winningPopup.SetActive(false);

        SceneManager.LoadScene("StartMenu");
    }

}
