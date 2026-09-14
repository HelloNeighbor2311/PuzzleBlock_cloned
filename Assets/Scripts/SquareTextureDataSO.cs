using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class SquareTextureDataSO : ScriptableObject
{
    [Serializable]
    public class TextureData
    {
        public Sprite texture;
        public Config.SquareColor squareColor;
    }

    public int thresholdVal = 10;
    private const int START_THRESHOLD_VALUE = 10;
    public List<TextureData> activeSquareTextures ;

    public Config.SquareColor currentColor;
    private Config.SquareColor nextColor;

    public int GetCurrentColorIndex()
    {
        var currentIndex = 0;
        for(int i = 0; i < activeSquareTextures.Count; i++)
        {
            if(activeSquareTextures[i].squareColor == currentColor)
            {
                currentIndex = i;
            }
        }
        return currentIndex;
    }

    public void UpdateColors(int currentScore)
    {
        currentColor = nextColor;
        var currentColorIndex = GetCurrentColorIndex();

        if(currentColorIndex == activeSquareTextures.Count - 1)
        {
            nextColor = activeSquareTextures[0].squareColor;
        }
        else
        {
            nextColor = activeSquareTextures[currentColorIndex+1].squareColor;
        }
        thresholdVal = START_THRESHOLD_VALUE + currentScore;
    }

    public void SetStartColor()
    {
        thresholdVal = START_THRESHOLD_VALUE;
        currentColor = activeSquareTextures[0].squareColor;
        nextColor = activeSquareTextures[1].squareColor;
    }

    private void Awake()
    {
        SetStartColor();
    }

    void OnEnable()
    {
        SetStartColor();
    }
}
