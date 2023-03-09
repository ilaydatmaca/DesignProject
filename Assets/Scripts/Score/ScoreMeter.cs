using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ScoreMeter : MonoBehaviour
{
    public Slider slider;

    public ScoreStar[] scoreStars = new ScoreStar[3];
    
    LevelGoal _levelGoal;
    int _maxScore;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // position the ScoreStars automatically
    public void SetupStars(LevelGoal levelGoal)
    {
        _levelGoal = levelGoal;
        _maxScore = _levelGoal.scoreGoals[_levelGoal.scoreGoals.Length - 1];

        // get the slider's RectTransform width
        float sliderWidth = slider.GetComponent<RectTransform>().rect.width;

        // avoid divide by zero error
        if (_maxScore > 0)
        {
            // loop through our scoring goals
            for (int i = 0; i < levelGoal.scoreGoals.Length; i++)
            {
                // if the corresponding ScoreStar exists...
                if (scoreStars[i] != null)
                {
                    // set the x value based on the ratio of the scoring goal over the maximum score
                    float newX = (sliderWidth * levelGoal.scoreGoals[i] / _maxScore) - (sliderWidth * 0.5f);

                    // move the ScoreStar's RectTransform
                    RectTransform starRectXform = scoreStars[i].GetComponent<RectTransform>();

                    if (starRectXform != null)
                    {
                        starRectXform.anchoredPosition = new Vector2(newX, starRectXform.anchoredPosition.y);
                    }
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
            slider.value = (float) score / (float) _maxScore;
        }

        // activate each star based on current star count
        for (int i = 0; i < starCount; i++)
        {
            if (scoreStars[i] != null)
            {
                scoreStars[i].Activate();
            }
        }
    }
}
