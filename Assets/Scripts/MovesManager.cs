
using UnityEngine;
using UnityEngine.UI;

public class MovesManager : MonoBehaviour
{
    private LevelGoal _levelGoal;
    
    public GameObject movesCounterArea;
    public Text _movesLeftText;

    private void Awake()
    {
        _levelGoal = FindObjectOfType<GameManager>().GetComponent<LevelGoal>();

    }

    private void Start()
    {
        EnableMovesCounterArea();
        UpdateMovesText();
    }
    
    
    void EnableMovesCounterArea()
    {
        if (movesCounterArea != null)
        {
            movesCounterArea.SetActive(true);
        }
    }
    
    public void DecreaseMoveLeft()
    {
        _levelGoal.movesLeft--;

        UpdateMovesText();
    }


    void UpdateMovesText()
    {
        if (_movesLeftText != null)
        {
            _movesLeftText.text = _levelGoal.movesLeft.ToString();
        }
    }
}
