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
    private bool newBestScore = false;
    private BestScoreData bestScoreData = new BestScoreData();
    public TextMeshProUGUI score;
    private int currentScore;
    private string bestScoreKey = "bsdat";

    private IEnumerator ReadDataFile()
    {
        bestScoreData = BinaryDataStream.Read<BestScoreData>(bestScoreKey);
        yield return new WaitForEndOfFrame();
        Debug.Log("Read best score: " + bestScoreData.score);
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
            //We might need to change when to update new bestscore into SaveBestScoreFunction instead of updating it everytime we scores
        }
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        score.text = currentScore.ToString();
    }
}
