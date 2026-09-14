using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSquareImageSelector : MonoBehaviour
{
   public SquareTextureDataSO squareTextureDataSO;
   public bool updateImageOnReachedThreshold = false;

    private void OnEnable()
    {
        UpdateSquareColorBaseOnCurrentPoints();
        if (updateImageOnReachedThreshold)
        {
            GameEvent.UpdateSquareColor += UpdateSquareColor;
        }
    }
    private void OnDisable()
    {
        if (updateImageOnReachedThreshold)
        {
            GameEvent.UpdateSquareColor -= UpdateSquareColor;
        }
    }
    private void UpdateSquareColorBaseOnCurrentPoints()
    {
        foreach(var i in squareTextureDataSO.activeSquareTextures)
        {
            if(squareTextureDataSO.currentColor == i.squareColor)
            {
                GetComponent<Image>().sprite = i.texture;
            }
        }
    }
    private void UpdateSquareColor(Config.SquareColor color)
    {
        foreach(var i in squareTextureDataSO.activeSquareTextures)
        {
            if(color == i.squareColor)
            {
                GetComponent<Image>().sprite = i.texture;
            }
        }
    }
}
