using UnityEngine;


public class MessageWindow : MonoBehaviour 
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


}
