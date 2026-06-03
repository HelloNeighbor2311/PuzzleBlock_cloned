using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour 
{
    public Image hoverImage;
    public Image activeImage;
    public Image normalImage;
    public List<Sprite> normalImages;
    public bool Selected {get; set;}
    public int SquareIndex {get; set;}
    public bool SquareOccupied {get; set;}


    void Start()
    {
        Selected = false;
        SquareOccupied = false;
        hoverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(false);
    }
    public void ActivateSquare()
    {
        hoverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        Selected = false;
        SquareOccupied = true;
    }
    public void PlaceShapeOnBoard()
    {
        ActivateSquare();
    }
    public void setFirstImage(bool isFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = isFirstImage ? normalImages[0] : normalImages[1];
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(SquareOccupied == false && other.GetComponent<ShapeSquare>() != null)
        {
            Selected = true;
            hoverImage.gameObject.SetActive(true);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(SquareOccupied == false && other.GetComponent<ShapeSquare>() != null)
        {
            Selected = true;
            hoverImage.gameObject.SetActive(true);
        }else if(other.GetComponent<ShapeSquare>()!= null){
            other.GetComponent<ShapeSquare>().SetOccupied();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
         if(SquareOccupied == false && other.GetComponent<ShapeSquare>() != null)
        {
            Selected = false;
            hoverImage.gameObject.SetActive(false);
        }else if(other.GetComponent<ShapeSquare>()!= null){
            other.GetComponent<ShapeSquare>().UnSetOccupied();
        }
    }
    //temp Function
    public bool CanBeReplaced()
    {
        return hoverImage.gameObject.activeSelf && !SquareOccupied;
    }
}
