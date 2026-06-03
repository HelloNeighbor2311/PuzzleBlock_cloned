using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int rows = 0;
    public int cols = 0;
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.1f;

    public GameObject gridSquare;
    public Vector2 startPos = new Vector2(0,0);
    
    private Vector2 offset = new Vector2(0,0);
    private List<GameObject> listGridSquare = new List<GameObject>();
    private LineIndicator lineIndicator;
    private void OnEnable()
    {
        GameEvent.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
    }

    private void OnDisable()
    {
        GameEvent.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
    }
    private void CheckIfShapeCanBePlaced()
    {
        var squareIndexes = new List<int>();
        foreach(var square in listGridSquare)
        {
            var squareValue = square.GetComponent<GridSquare>();
            if(squareValue.Selected && !squareValue.SquareOccupied)
            {
                squareIndexes.Add(squareValue.SquareIndex);
                squareValue.Selected = false;
                //squareValue.ActivateSquare();
            }
        }
        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if(currentSelectedShape == null) return; //There is no selected shape

        if(currentSelectedShape.TotalSquareNumber == squareIndexes.Count)
        {
            foreach(var index in squareIndexes){
                listGridSquare[index].GetComponent<GridSquare>().PlaceShapeOnBoard();
            }

            int shapeLeft = 0;
            foreach(var shape in shapeStorage.shapeList)
            {
                if(shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive()) shapeLeft++;
            }
        //     Debug.Log("Shape left: " + shapeLeft);
            //currentSelectedShape.DeactivateShape();
            if(shapeLeft == 0)
            {
                GameEvent.RequestNewShapes?.Invoke();
            }
            else
            {
                GameEvent.SetShapeInActive?.Invoke();
            }
        
        }else{
            GameEvent.MoveShapeToStartPosition?.Invoke();
        }
    }

    private void Start()
    {
        lineIndicator = GetComponent<LineIndicator>();
        SpawnGridSquare();
        SetGridSquarePosition();
    }

    private void SpawnGridSquare()
    {
        int square_index = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                listGridSquare.Add(SimplePool2.Spawn(gridSquare));
                listGridSquare[listGridSquare.Count - 1].GetComponent<GridSquare>().SquareIndex = square_index;
                listGridSquare[listGridSquare.Count - 1].transform.SetParent(this.transform);
                listGridSquare[listGridSquare.Count-1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                listGridSquare[listGridSquare.Count - 1].GetComponent<GridSquare>()
                .setFirstImage(lineIndicator.GetGridSquareIndex(square_index) % 2 == 0);
                square_index++;
            }
        }
    }
    private void SetGridSquarePosition()
    {
        int row_num = 0;
        int col_num = 0;
        Vector2 squareGapNum = new Vector2(0,0);

        var squareRect = listGridSquare[0].GetComponent<RectTransform>();

        offset.x = squareRect.rect.width * squareRect.transform.localScale.x + everySquareOffset;   
        offset.y = squareRect.rect.height * squareRect.transform.localScale.y + everySquareOffset;   

        foreach(GameObject square in listGridSquare)
        {
            if(col_num + 1 > cols)
            {
                squareGapNum.x = 0;
                col_num = 0;
                row_num++;
            }

            var pos_x_offset = col_num * offset.x + squareGapNum.x;
            var pos_y_offset = row_num * offset.y + squareGapNum.y;

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x + pos_x_offset, startPos.y - pos_y_offset);
            col_num++;
        }

    }

}
