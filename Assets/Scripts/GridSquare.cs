using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour 
{
    public Image normalImage;
    public List<Sprite> normalImages;
    public void setFirstImage(bool isFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = isFirstImage ? normalImages[0] : normalImages[1];
    }
}
