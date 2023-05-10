using UnityEngine;
using TMPro;

public class UpdateCurrencies : MonoBehaviour
{
    public TMP_Text coinValueText;
    public TMP_Text trophiesValueText;

    void Start()
    {
        Updatecoins();
        UpdateStars();
    }
    
    public void Updatecoins()
    {
        coinValueText.text = PlayFabManager.coins.ToString();
    }
    
    public void UpdateStars()
    {
        trophiesValueText.text = PlayFabManager.trophies.ToString();
    }
}
