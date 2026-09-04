using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;   


    private void OnEnable()
    {
        GameEvent.RequestNewShapes += RequestNewShapes;
    }
    private void OnDisable()
    {
        GameEvent.RequestNewShapes -= RequestNewShapes;
    }

    private void RequestNewShapes()
    {
        foreach(var shape in shapeList)
        {
            int shapeIndex = UnityEngine.Random.Range(0,shapeData.Count);
            shape.RequestNewShape(shapeData[shapeIndex]);
            Debug.Log("Shape " + shape.name + " created with data: " + shapeData[shapeIndex].name);
        }    
    }

    void Start()
    {
        foreach(var shape in shapeList)
        {
            int shapeIndex = UnityEngine.Random.Range(0,shapeData.Count);
            shape.CreateShape(shapeData[shapeIndex]);
            Debug.Log("Shape " + shape.name + " created with data: " + shapeData[shapeIndex].name);
        }    
    }
    public Shape GetCurrentSelectedShape()
    {
        foreach(var shape in shapeList)
        {
            if(shape.IsOnStartPosition() == false && shape.IsAnyOfShapeSquareActive())
            {
                return shape;
            }
        }
        return null;
    }
}
