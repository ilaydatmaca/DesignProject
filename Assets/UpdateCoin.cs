using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCoin : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text starText;

    private void Start()
    {
        Updatecoins();
    }

    public void Updatecoins()
    {
        int coins = PlayFabLogin.coins;
        coinText.text = coins.ToString();

        int stars = PlayFabLogin.stars;
        starText.text = stars.ToString();

    }

}
