using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


[Serializable]
public class BestScoreData
{
    public int score = 0;
}
public class Score : MonoBehaviour
{
    public SquareTextureDataSO squareTextureDataSO;
    private bool newBestScore = false;
    private BestScoreData bestScoreData = new BestScoreData();
    public TextMeshProUGUI score;
    private int currentScore;
    private string bestScoreKey = "bsdat";

    private IEnumerator ReadDataFile()
    {
        bestScoreData = BinaryDataStream.Read<BestScoreData>(bestScoreKey);
        yield return new WaitForEndOfFrame();
        GameEvent.UpdateBestScoreBar(currentScore, bestScoreData.score);
    }
    private void Awake()
    {
        if (BinaryDataStream.Exist(bestScoreKey))
        {
            StartCoroutine(ReadDataFile());
        }
    }
    private void Start()
    {
        currentScore = 0;
        newBestScore = false;
        squareTextureDataSO.SetStartColor();
        UpdateScoreText();
    }
    private void OnEnable()
    {
        GameEvent.AddScores += AddScores;
        GameEvent.GameOver += SaveBestScore;
    }
    private void OnDisable()
    {
        GameEvent.AddScores -= AddScores;
        GameEvent.GameOver -= SaveBestScore;
    }
    private void SaveBestScore(bool newBestScore)
    {
        BinaryDataStream.Save<BestScoreData>(bestScoreData, bestScoreKey);
    }
    private void AddScores(int scoresToAdd)
    {
        currentScore += scoresToAdd;
        if(currentScore > bestScoreData.score)
        {
            newBestScore = true;
            bestScoreData.score = currentScore;
            SaveBestScore(true);
            //We might need to change when to update new bestscore into SaveBestScoreFunction instead of updating it everytime we scores
        }
        UpdateSquareColor();
        GameEvent.UpdateBestScoreBar(currentScore, bestScoreData.score);
        UpdateScoreText();
    }
    private void UpdateSquareColor()
    {
        if(GameEvent.UpdateSquareColor != null && currentScore >= squareTextureDataSO.thresholdVal)
        {
            squareTextureDataSO.UpdateColors(currentScore);
            GameEvent.UpdateSquareColor(squareTextureDataSO.currentColor);
        }
    }
    private void UpdateScoreText()
    {
        score.text = currentScore.ToString();
    }
}
