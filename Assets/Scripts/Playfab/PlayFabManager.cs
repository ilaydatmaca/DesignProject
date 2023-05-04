using System;
using System.Collections.Generic;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{
    public PlayFabManager instance;
    public static int coins, stars;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.LogFormat("Player {0} login/account successfully!", result.PlayFabId);
        string name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        GetVirtualCurrencies();
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        coins = result.VirtualCurrency["CN"];
        stars = result.VirtualCurrency["ST"];
        //updatecoins.Updatecoins(coins);
        //updatecoins.UpdateStars(stars);

    }

    public void GrantVirtualCurrency()
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = 50
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnGrantVirtualCurrencySuccess, OnError);
    }

    void OnGrantVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Currency Granted!");
        GetVirtualCurrencies();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        Debug.Log("Here's some debug informations:");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Score",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardSuccess, OnLeaderboardFailure);
    }

    void OnLeaderboardSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent");
    }
    
    void OnLeaderboardFailure(PlayFabError error)
    {
        Debug.LogWarning("Failed to update leaderbord");
        Debug.Log(error.GenerateErrorReport());
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Score",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request,  OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }

    void SubmitName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            //DisplayName = name
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnDisplayNameFailure);
    }
    
    void OnDisplayNameFailure(PlayFabError error)
    {
        Debug.LogWarning("Failed to update name");
        Debug.Log(error.GenerateErrorReport());
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
    }
    
}