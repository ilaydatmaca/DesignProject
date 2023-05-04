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
    //public TMP_Text Text;
    public Slider slider;

    private Board _board;
    private ClearManager _clearManager;
    private void Awake()
    {
        _board = FindObjectOfType<Board>().GetComponent<Board>();
        _clearManager = FindObjectOfType<ClearManager>().GetComponent<ClearManager>();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        ResetTime();
        Initialize();
    }
    
    public void Update()
    {
        if (!isTimerRunning || GameManager.Instance.paused || RoundManager.Instance.roundComplete) return;
        
        _currentTime = TimeRemaining();
        //Text.text = _currentTime.ToString();
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
        
        while (_board.isRefilling && !_board.playerInputEnabled)
        {
            yield return null;
        }

        RoundManager.Instance.CheckAllRoundsComplete();
        
        yield return new WaitForSeconds(RoundManager.Instance.waitTimeForRounds);

        ResetTime();

        if (PhotonNetwork.IsMasterClient)
        {
            SetStartTime();
        }
        
        RoundManager.Instance.UpdateRound();
        MovesManager.Instance.Init();

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


    public void AddTime(int bonusTime)
    {
        float clampTime = Mathf.Clamp(bonusTime, 0f, maxTime- TimeRemaining());
        
        startTime += (int)clampTime * 1000;
    }
    public static void SetStartTime()
    {
        int startTime = 0;
        bool wasSet = TryGetStartTime(out startTime);

        Hashtable props = new Hashtable { { CountdownStartTime, PhotonNetwork.ServerTimestamp } };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
}