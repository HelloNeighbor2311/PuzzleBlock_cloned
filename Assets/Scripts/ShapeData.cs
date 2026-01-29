using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ShapeData : ScriptableObject
{
    [System.Serializable]
    public class Row //A row with specific number of column inside
    {
        public bool[] column;
        private int size = 0; //number of column
        
        public Row(){}
        public Row(int size)
        {
            createRow(size);
        }
        public void createRow(int size)
        {
            this.size = size;
            column = new bool[size];
            clearRow();
        }
        public void clearRow()
        {
            for (int i = 0; i < size; i++)
            {
                column[i] = false;
            }
        }
    }

    public int columns = 0;
    public int rows = 0;
    public Row[] board; //an editable board to make shape

    public void Clear()
    {
        for(var i =0; i < rows; i++)
        {
            board[i].clearRow();
        }
    }
    public void CreateNewBoard()
    {
        board = new Row[rows];
        for(var i = 0;i < rows; i++)
        {
            board[i] = new Row(columns);

        }
    }

}
