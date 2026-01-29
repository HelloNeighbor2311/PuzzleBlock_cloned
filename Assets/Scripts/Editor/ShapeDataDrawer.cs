using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ShapeData),false)] //Enable to custom the inspector of ShapeData scriptable object (main purpose of this script)
[CanEditMultipleObjects] 
[System.Serializable]
public class ShapeDataDrawer : Editor
{
    private ShapeData ShapeDataInstance => target as ShapeData;

    public override void OnInspectorGUI() // == Update() but work only in edit mode, practically in Inspector
    {
        serializedObject.Update(); //Begin function
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnsInputField();
        EditorGUILayout.Space();

        if(ShapeDataInstance != null && ShapeDataInstance.columns > 0 && ShapeDataInstance.rows > 0)
        {
            DrawBoardTable();
        }
        serializedObject.ApplyModifiedProperties(); //apply all changes
        if (GUI.changed) //mark objects which changed to dirty (need to save)
        {
            EditorUtility.SetDirty(ShapeDataInstance); //End function
        }

    }

    private void ClearBoardButton() //make a clear board button on inspector
    {
        if (GUILayout.Button("ClearBoard"))
        {
            ShapeDataInstance.Clear();
        }
    }
    
    private void DrawColumnsInputField() //input field(mattrix) to draw data square in it
    {
        var colTemp = ShapeDataInstance.columns;
        var rowTemp = ShapeDataInstance.rows;

        ShapeDataInstance.columns = EditorGUILayout.IntField("Columns", ShapeDataInstance.columns);
        ShapeDataInstance.rows = EditorGUILayout.IntField("Rows", ShapeDataInstance.rows);
        
        if((ShapeDataInstance.columns != colTemp || ShapeDataInstance.rows != rowTemp) &&
            ShapeDataInstance.columns > 0 && ShapeDataInstance.rows > 0)
        {
            ShapeDataInstance.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box"); //The whole table
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle(); //column style
        headerColumnStyle.fixedWidth = 65;
        headerColumnStyle.alignment = TextAnchor.MiddleCenter;

        var rowStyle = new GUIStyle(); //row style
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid); //data square will appear as a toggle button (true or false value)
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for (var row = 0; row < ShapeDataInstance.rows; row++) //loop the mattrix to create box shape with data square inside
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);
            for(var col = 0; col < ShapeDataInstance.columns; col++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(ShapeDataInstance.board[row].column[col], dataFieldStyle); //make a button with existed value and style
                ShapeDataInstance.board[row].column[col] = data;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
