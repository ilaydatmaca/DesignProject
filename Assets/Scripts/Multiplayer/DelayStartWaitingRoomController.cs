using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayStartWaitingRoomController : MonoBehaviourPunCallbacks
{
    private PhotonView myPhotonView;

    [SerializeField] private int multiplayerSceneIndex;
    [SerializeField] private int menuSceneIndex;

    private int playerCount;
    private int roomSize;
    [SerializeField] private int minPlayersToStart;
    [SerializeField] private TMP_Text roomCountDisplay;
    [SerializeField] private TMP_Text timerToStartDisplay;

    private bool readyToCountDown;
    private bool readyToStart;
    private bool startingGame;
    
    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;

    [SerializeField] private float maxWaitTime;
    [SerializeField] private float maxFullGameWaitTime;


    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameWaitTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();

    }

    void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        roomCountDisplay.text = playerCount + " : " + roomSize;

        if (playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if (playerCount >= minPlayersToStart)
        {
            readyToCountDown = true;
        }
        else
        {
            readyToCountDown = false;
            readyToStart = false;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();
        if(PhotonNetwork.IsMasterClient)
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if (timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    private void Update()
    {
        WaitingForMorePlayers();
    }

    void WaitingForMorePlayers()
    {
        if (playerCount <= 1)
        {
            ResetTimer();
        }

        if (readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (readyToCountDown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartDisplay.text = tempTimer;

        if (timerToStartGame <= 0f)
        {
            if (startingGame)
                return;
            StartGame();
        }
    }

    void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameWaitTime;
    }

    void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerSceneIndex);
    }

    public void DelayCancel()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuSceneIndex);
    }
}
