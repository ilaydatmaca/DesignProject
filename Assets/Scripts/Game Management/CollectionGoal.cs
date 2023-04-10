using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : MonoBehaviour
{
    private int _countGoal = 6;

    private MatchValue _matchValue;
    private void Start()
    {
        if (RoundManager.Instance.player1View.IsMine)
        {
            _matchValue = MatchValue.Blue;
        }
        else if (RoundManager.Instance.player2View.IsMine)
        {
            _matchValue = MatchValue.Red;
        }
    }

    private void UpdateGoal(MatchValue matchValue)
    {
        if (RoundManager.Instance.turnView.IsMine && matchValue == _matchValue)
        {
            _countGoal = 6;
        }
    }
}
