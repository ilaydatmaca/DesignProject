using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonTimer : MonoBehaviourPunCallbacks
{
    public delegate void CountdownTimerHasExpired();
    public static event CountdownTimerHasExpired OnCountdownTimerHasExpired;
    public const string CountdownStartTime = "StartTime";
    
    private bool isTimerRunning;
    private int startTime;
    private float _currentTime;
    
    public float maxTime = 30f;
    public TMP_Text Text;
    public Slider slider;
    
    public override void OnEnable()
    {
        base.OnEnable();

        ResetTime();
        Initialize();
    }
    
    public void Update()
    {
        if (!isTimerRunning) return;
        
        _currentTime = TimeRemaining();
        slider.value = _currentTime;

        if (MovesManager.Instance.noMoreMoves)
            _currentTime = 0.0f;
        
        if (_currentTime > 0.0f) return;

        OnTimerEnds();
    }

    private void ResetTime()
    {
        slider.maxValue = maxTime;
        slider.value = maxTime;
    }

    private void OnTimerRuns()
    {
        isTimerRunning = true;
        enabled = true;
    }

    public void OnTimerEnds()
    {
        isTimerRunning = false;

        StartCoroutine(CoroutineTime());

        if (OnCountdownTimerHasExpired != null) OnCountdownTimerHasExpired();
    }

    IEnumerator CoroutineTime()
    {
        GameManager.Instance.paused = true;
        ResetTime();
        MovesManager.Instance.Init();

        yield return new WaitForSeconds(RoundManager.Instance.waitTimeForRounds);
        
        SetStartTime();
        RoundManager.Instance.UpdateRound();
        GameManager.Instance.paused = false;
    }
    
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Initialize();
    }


    private void Initialize()
    {
        int propStartTime;
        if (TryGetStartTime(out propStartTime))
        {
            startTime = propStartTime;

            isTimerRunning = TimeRemaining() > 0;

            if (isTimerRunning)
                OnTimerRuns();
            else
                OnTimerEnds();
        }
    }
    
    private float TimeRemaining()
    {
        int timer = PhotonNetwork.ServerTimestamp - startTime;
        return maxTime - timer / 1000f;
    }

    public static bool TryGetStartTime(out int startTimestamp)
    {
        startTimestamp = PhotonNetwork.ServerTimestamp;

        object startTimeFromProps;
        
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CountdownStartTime, out startTimeFromProps))
        {
            startTimestamp = (int)startTimeFromProps;
            return true;
        }

        return false;
    }


    public static void SetStartTime()
    {
        int startTime = 0;
        bool wasSet = TryGetStartTime(out startTime);

        Hashtable props = new Hashtable { { CountdownStartTime, PhotonNetwork.ServerTimestamp } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
}