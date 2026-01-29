using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] private GameObject squareShapeImage;

    //[HideInInspector] 
    public ShapeData currentShapeData;

    private List<GameObject> currentShapeList = new List<GameObject>();
    private void Start()
    {
        RequestNewShape(currentShapeData);    
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        CreateShape(shapeData);
    }

    public void CreateShape(ShapeData shapeData)
    {
        currentShapeData = shapeData;
        var totalSquareNumber = GetNumberOfSquare(shapeData);

        while (currentShapeList.Count <= totalSquareNumber) {
            currentShapeList.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }

        foreach (var square in currentShapeList) { 
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false); 
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;
        for (var row = 0; row < shapeData.rows; row++) { //set positions to form final shape
            for (var col = 0; col < shapeData.columns; col++) {
                if (shapeData.board[row].column[col])
                {
                    currentShapeList[currentIndexInList].SetActive(true);
                    currentShapeList[currentIndexInList].GetComponent<RectTransform>().localPosition =
                        new Vector2(GetXPositionForShapeSquare(shapeData, col, moveDistance), GetYPositionForShapeSquare(shapeData, row, moveDistance));

                    currentIndexInList++;
                }
            }
        }
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;
        if (shapeData.rows > 1)
        {
            if (shapeData.rows%2 != 0) {
                var middleSquareIndex = (shapeData.rows-1)/2;
                var multiplier = (shapeData.rows-1)/2;

                if(row < middleSquareIndex) //move it on minus
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if(row > middleSquareIndex) //move it on plus
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex1 =  (shapeData.rows == 2) ? 0 : (shapeData.rows-1);
                var middleSquareIndex2 =  (shapeData.rows == 2) ? 1 : (shapeData.rows/2);
                var multiplier = shapeData.rows/2;

                if(row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if(row == middleSquareIndex2)
                    {
                        shiftOnY = (moveDistance.y / 2) * -1;
                    }
                    if(row == middleSquareIndex1)
                    {
                        shiftOnY = (moveDistance.y/2) * 1;
                    }
                }if(row < middleSquareIndex1 && row < middleSquareIndex2) //move it on minus
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }else if(row > middleSquareIndex1 && row > middleSquareIndex2) //move it on plus
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
        }
        return shiftOnY;
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        float shiftOnX = 0.0f;
        if(shapeData.columns > 1) { //vertical position caculation 
            if (shapeData.columns % 2 != 0)
            {
                var middleSquareIndex = (shapeData.columns - 1) / 2;
                var multiplier = (shapeData.columns - 1) / 2;
                if (column < middleSquareIndex)
                { //move it on the negative 
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if (column > middleSquareIndex)
                { //move it on the plus 
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.columns == 2) ? 1 : (shapeData.columns / 2);
                var middleSquareIndex1 = (shapeData.columns == 2) ? 0 : (shapeData.columns - 1);
                var multiplier = shapeData.columns / 2;

                if (column == middleSquareIndex2 || column == middleSquareIndex1)
                {
                    if (column == middleSquareIndex2)
                    {
                        shiftOnX = moveDistance.x / 2;
                    }
                    if (column == middleSquareIndex1)
                    {
                        shiftOnX = (moveDistance.x / 2) * -1;
                    }
                }
                if(column < middleSquareIndex1 && column < middleSquareIndex2) //move it on negative
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if(column > middleSquareIndex1 && column > middleSquareIndex2) //move it on plus
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
        }
        return shiftOnX;
    }
    private int GetNumberOfSquare(ShapeData shapeData)
    {
        int number = 0;
        foreach(var rowData in shapeData.board)
        {
            foreach(var actived in rowData.column)
            {
                if (actived)
                {
                    number++;   
                }
            }
        }

         return number;
    }

}
