using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCoin : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text starText;

    
    public void Updatecoins(int coins)
    {
        coinText.text = coins.ToString();
    }


    public void UpdateStars(int stars)
    {
        starText.text = stars.ToString();
    }

}
