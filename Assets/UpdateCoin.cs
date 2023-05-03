using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCoin : MonoBehaviour
{
    public TMP_Text coinText;
    public TMP_Text starText;

    void Start()
    {
        Updatecoins();
        UpdateStars();
    }


    public void Updatecoins()
    {
        coinText.text = PlayFabLogin.coins.ToString();
    }


    public void UpdateStars()
    {
        starText.text = PlayFabLogin.stars.ToString();
    }

}
