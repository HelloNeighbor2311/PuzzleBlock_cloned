using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI score;
    private int currentScore;
    private void Start()
    {
        currentScore = 0;
        UpdateScoreText();
    }
    private void OnEnable()
    {
        GameEvent.AddScores += AddScores;
    }
    private void OnDisable()
    {
        GameEvent.AddScores -= AddScores;
    }
    private void AddScores(int scoresToAdd)
    {
        currentScore += scoresToAdd;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        score.text = currentScore.ToString();
    }
}
