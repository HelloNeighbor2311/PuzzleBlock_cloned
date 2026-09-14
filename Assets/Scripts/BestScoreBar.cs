using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreBar : MonoBehaviour
{
    public Image fillInImage;
    public TextMeshProUGUI bestScoreText;
    private void OnEnable()
    {
        GameEvent.UpdateBestScoreBar += OnUpdateBestScoreBar;
    }
    private void OnDisable()
    {
        GameEvent.UpdateBestScoreBar -= OnUpdateBestScoreBar;
    }
    private void OnUpdateBestScoreBar(int currentScore, int bestScore)
    {
        float currentPercentage = (float) currentScore/ bestScore;
        fillInImage.fillAmount = currentPercentage;
        bestScoreText.text = bestScore.ToString();
    }
}
