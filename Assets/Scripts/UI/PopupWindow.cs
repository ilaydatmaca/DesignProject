using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupWindow : Singleton<PopupWindow>
{
    public GameObject losingPopup;
    public GameObject winningPopup;
    public GameObject drawPopup;
    
    public TMP_Text losingScoreText;
    public TMP_Text winningScoreText;
    public TMP_Text drawScoreText;

    public void ShowLoseWindow()
    {
        losingScoreText.text = ScoreManager.Instance.GetMyScore().ToString();
        losingPopup.SetActive(true);
    }
    
    public void ShowWinWindow()
    {
        winningScoreText.text = ScoreManager.Instance.GetMyScore().ToString();
        winningPopup.SetActive(true);
    }
    
    public void ShowDrawWindow()
    {
        drawScoreText.text = ScoreManager.Instance.GetMyScore().ToString();
        drawPopup.SetActive(true);
    }

    public void Restart()
    {
        losingPopup.SetActive(false);
        winningPopup.SetActive(false);

        SceneManager.LoadScene("StartMenu");
    }

}
