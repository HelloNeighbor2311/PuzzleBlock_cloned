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
    private float squareIncreaseScale = 0.05f;
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
            CheckAnyLineIsCompleted();
        
        }else{
            GameEvent.MoveShapeToStartPosition?.Invoke();
        }
    }

    private void CheckAnyLineIsCompleted()
    {
        List<int[]> lines = new List<int[]>();


        //columns
        foreach(var column in lineIndicator.columnIndexes)
        {
            lines.Add(lineIndicator.getVerticalLine(column));
        }

        //rows
        for(int row = 0; row < 9; row++)
        {
            List<int> data = new List<int>(9);
            for(int i = 0; i < 9; i++)
            {
                data.Add(lineIndicator.line_Data[row,i]);
            }
            lines.Add(data.ToArray());
        }

        var completedLine = CheckIfSquaresAreCompleted(lines);
        if(completedLine> 2)
        {
            //TODO: Play bonus animation
        }
        
        //Todo: Add score based on completedLine
        var totalScores = 10 * completedLine;
        GameEvent.AddScores?.Invoke(totalScores);
        CheckIfPlayerLost();
    }
    private int CheckIfSquaresAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();
        int linesCompleted = 0;
        foreach(var line in data)
        {
           var isLineCompleted = true;
           foreach(var squareIndex in line)
            {
                var comp = listGridSquare[squareIndex].GetComponent<GridSquare>();
                if(comp.SquareOccupied == false)
                {
                    isLineCompleted = false;
                }
            } 

            if(isLineCompleted)
            {
                completedLines.Add(line);
            }
        }
        foreach(var line in completedLines)
        {
            var completed = false;
            foreach(var squareIndex in line)
            {
                var comp = listGridSquare[squareIndex].GetComponent<GridSquare>();
                comp.DeactivateSquare();
                completed = true;
            }
            foreach(var squareIndex in line)
            {
                var comp = listGridSquare[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }
            if(completed)
            {
                linesCompleted++;
            }
        }
        return linesCompleted;
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

                //Set scale of the grid square and increase it by a certain amount
                listGridSquare[listGridSquare.Count-1].transform.localScale = new Vector3(squareScale + squareIncreaseScale, squareScale + squareIncreaseScale, squareScale + squareIncreaseScale);
                
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

        //Decrease the certain amount that has been increased to get the original size of the grid square
        squareRect.localScale -= new Vector3(squareIncreaseScale, squareIncreaseScale, squareIncreaseScale);
        
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
    private void CheckIfPlayerLost()
    {
        var validShape = 0;
        for(int index = 0; index < shapeStorage.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorage.shapeList[index].IsAnyOfShapeSquareActive();
            if(CheckIfShapeCanBePlaceOnGrid(shapeStorage.shapeList[index]) && isShapeActive)
            {
                shapeStorage.shapeList[index]?.ActivateShape();
                validShape++;
            }
        }
        if(validShape == 0)
        {
            //Game over event
            GameEvent.GameOver(false);
            //Debug.LogWarning("You Lose. GAME OVER");
        }
    }
    private bool CheckIfShapeCanBePlaceOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.currentShapeData;
        var shapeColumns = currentShapeData.columns;
        var shapeRows = currentShapeData.rows;

        //All indexes of filled up squares
        List<int> originalShapeFilledUpSquares = new List<int>();
        var squareIndex = 0;

        for(var rowI = 0; rowI < shapeRows; rowI++)
        {
            for(var colI = 0; colI < shapeColumns; colI++)
            {
                if (currentShapeData.board[rowI].column[colI])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }
                squareIndex++;
            }
        }
        if(currentShape.TotalSquareNumber != originalShapeFilledUpSquares.Count)
        {
            Debug.LogError("Number of filled up squares are not the same as the original shape has");
        }

        var squareList = GetAllSquaresCombination(shapeColumns, shapeRows);
        bool canBePlaced = false;

        foreach(var number in squareList)
        {
            bool shapeCanBePlaceOnTheBoard = true;
            foreach(var squareIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = listGridSquare[number[squareIndexToCheck]].GetComponent<GridSquare>();
                if (comp.SquareOccupied == true)
                {
                    shapeCanBePlaceOnTheBoard = false;
                }
            }

            if (shapeCanBePlaceOnTheBoard)
            {
                canBePlaced = true;
            }
        }
        return canBePlaced;
    }

    private List<int[]> GetAllSquaresCombination(int columns, int rows)
    {
        var squareList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;
        while(lastRowIndex + (rows-1) < 9)
        {
            var rowData = new List<int>();
            for(var row = lastRowIndex; row < lastRowIndex + rows; row++)
            {
                for(var column = lastColumnIndex; column< lastColumnIndex+ columns; column++)
                {
                    rowData.Add(lineIndicator.line_Data[row,column]);
                }
            }
            squareList.Add(rowData.ToArray());

            lastColumnIndex++;

            if(lastColumnIndex + (columns - 1) >= 9)
            {
                lastRowIndex ++;
                lastColumnIndex = 0;

            }
            safeIndex++;
            if(safeIndex > 100)
            {
                break;
            }
        }

        return squareList;
    }
}
