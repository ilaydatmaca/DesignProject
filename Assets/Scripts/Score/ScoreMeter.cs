using UnityEngine;
using UnityEngine.UI;

public class ScoreMeter : MonoBehaviour
{
    private Slider _slider;
    private int _maxScore;
    private LevelGoal _levelGoal;

    public ScoreStar[] _scoreStars;
    

    void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    
    public void Init(LevelGoal levelGoal)
    {
        _levelGoal = levelGoal;
        SetupStars();
    }
    
    void SetupStars()
    {
        _maxScore = _levelGoal.scoreGoals[_levelGoal.scoreGoals.Length - 1];

        float sliderWidth = _slider.GetComponent<RectTransform>().rect.width;

        if (_maxScore > 0)
        {
            for (int i = 0; i < _levelGoal.scoreGoals.Length; i++)
            {
                if (_scoreStars[i] != null)
                {
                    float starXPos = (sliderWidth * _levelGoal.scoreGoals[i] / _maxScore) - (sliderWidth * 0.5f);
                    
                    RectTransform startRect = _scoreStars[i].GetComponent<RectTransform>();
                    
                    startRect.anchoredPosition = new Vector2(starXPos, startRect.anchoredPosition.y);
                }
            }

        }

    }

    // Update the ScoreMeter 
    public void UpdateScoreMeter(int score, int starCount)
    {
        if (_levelGoal != null)
        {
            // adjust the slider fill area (cast as floats, otherwise will become zero)
            _slider.value = (float) score / _maxScore;
        }

        // activate each star based on current star count
        for (int i = 0; i < starCount; i++)
        {
            if (_scoreStars[i] != null)
            {
                _scoreStars[i].Activate();
            }
        }
    }
}
