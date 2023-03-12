using UnityEngine;
using UnityEngine.UI;

public class ScoreMeter : MonoBehaviour
{
    private Slider _slider;

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    
    public void UpdateScoreMeter(int score)
    {
        _slider.value = (float) score / ScoreManager.Instance.maxScore;
    }
}
