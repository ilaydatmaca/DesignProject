using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Booster : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static GameObject ActiveBooster; // the one active Booster GameObject

    private Vector3 _startPosition; // reset position
    private Cell _targetCell;
    
    public bool isEnabled;    // is the Booster enabled? (has the button been clicked once?)
    public bool isDraggable = true;

    public UnityEvent boostEvent;
    public int bonusTime = 15;

    private Board _board;
    private SoundManager _soundManager;
    private RoundManager _roundManager;
    
    private RectTransform _rectform;
    public TMP_Text amountText;

    public int boosterCount = 1;


    void Awake()
    {
        _rectform = GetComponent<RectTransform>();
        _board = FindObjectOfType<Board>().GetComponent<Board>();
        _soundManager = FindObjectOfType<SoundManager>();
        _roundManager = FindObjectOfType<RoundManager>();
    }

    void Start()
    {
        EnableBooster(false);
    }

    void EnableBooster(bool state)
    {
        isEnabled = state;

        if (state)
        {
            DisableOtherBoosters();
            ActiveBooster = gameObject;
        }
        else if (gameObject == ActiveBooster)
        {
            ActiveBooster = null;
        }

    }

    void DisableOtherBoosters()
    {
        Booster[] allBoosters = FindObjectsOfType<Booster>();

        foreach (Booster b in allBoosters)
        {
            if (b != this)
            {
                b.EnableBooster(false);
            }
        }
    }

    public void ToggleBooster()
    {
        EnableBooster(!isEnabled);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEnabled && isDraggable && _roundManager.turnView.IsMine)
        {
            _startPosition = gameObject.transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isEnabled && isDraggable)
        {
            Vector3 onscreenPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectform, eventData.position, 
                                                                    Camera.main, out onscreenPosition);
            gameObject.transform.position = onscreenPosition;

            RaycastHit2D hit2D = Physics2D.Raycast(onscreenPosition, Vector3.forward, Mathf.Infinity);

            _targetCell = (hit2D.collider != null) ? hit2D.collider.GetComponent<Cell>() : null;
        }
    }

    // frame where we end drag
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isEnabled && isDraggable)
        {
            gameObject.transform.position = _startPosition;

            if (_board != null && _board.isRefilling)
            {
                return;
            }

            if (_targetCell != null)
            {
                if (boostEvent != null)
                {
                    boostEvent.Invoke();
                }

                EnableBooster(false);

                _targetCell = null;
                ActiveBooster = null;
            }
        }
    }
    
    public void DropColorBomb()
    {
        if (_targetCell != null && CollectionGoal.Instance.canUseBooster)
        {
            _board.photonView.RPC("RPC_MakeColorBombBooster", RpcTarget.AllBuffered, _targetCell.xIndex, _targetCell.yIndex);
            CollectionGoal.Instance.canUseBooster = false;

        }
    }
    
    public void RocketBooster()
    {
        if (_targetCell != null && CollectionGoal.Instance.canUseBooster)
        {
            _board.photonView.RPC("RPC_RemoveColumn", RpcTarget.AllBuffered, _targetCell.xIndex);
            CollectionGoal.Instance.canUseBooster = false;
        }
    }
    public void ShuffleBoard()
    {
        if (boosterCount > 0 && _roundManager.turnView.IsMine)
        {
            _board.photonView.RPC("RPC_Shuffle", RpcTarget.AllBuffered);
            UpdateText();
        }
    }
    
    public void RemoveOneGamePiece()
    {
        if (_targetCell != null && boosterCount > 0)
        {
            _board.photonView.RPC("RPC_RemoveOneGamePiece", RpcTarget.AllBuffered, _targetCell.xIndex, _targetCell.yIndex);
            UpdateText();
        }
    }

    
    public void AddTime()
    {
        if (_roundManager.turnView.IsMine && boosterCount > 0)
        {
            _board.photonView.RPC("RPC_AddTime", RpcTarget.AllBuffered, bonusTime);
            UpdateText();
        }
    }

    private void UpdateText()
    {
        boosterCount--;
        //_soundManager.PlayBonusSound();
        amountText.text = boosterCount.ToString();
    }
}
