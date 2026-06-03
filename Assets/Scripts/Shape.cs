using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] private GameObject squareShapeImage;
    [HideInInspector] public ShapeData currentShapeData;

    public int TotalSquareNumber {get; set;}

    private Vector3 shapeSelectedScale = new Vector3(0.8f, 0.8f, 0.8f) ;
    private Vector3 shapeStartScale;
    private Vector3 shapePlacedScale = new Vector3(0.7f, 0.7f, 0.7f);
    private Vector2 offset = new Vector2(0, 700);
    private List<GameObject> currentShapeList = new List<GameObject>();
    private RectTransform shapeRectTransform;
    private bool shapeDraggable = true;
    private Canvas canvas;
    private Vector2 startPosition;
    private Vector2 startAnchoredPosition;
    private Vector2 startAnchorMin;
    private Vector2 startAnchorMax;
    private Vector2 startPivot;
    private bool isShapeActive = true;

    private void Awake()
    {
        shapeStartScale = transform.localScale;
        shapeRectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        shapeDraggable = true;  
        startPosition = shapeRectTransform.anchoredPosition;
        startAnchoredPosition = shapeRectTransform.anchoredPosition;
        startAnchorMin = shapeRectTransform.anchorMin;
        startAnchorMax = shapeRectTransform.anchorMax;
        startPivot = shapeRectTransform.pivot;
        isShapeActive = true;
    }

    private void OnEnable()
    {
        GameEvent.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEvent.SetShapeInActive += SetShapeInActive;
    }
    private void OnDisable()
    {
        GameEvent.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvent.SetShapeInActive -= SetShapeInActive;
    }
    private void MoveShapeToStartPosition()
    {
        ResetShapeRectTransform();
    }
    public bool IsOnStartPosition()
    {
        return shapeRectTransform.anchoredPosition == startAnchoredPosition;
    }
    public bool IsAnyOfShapeSquareActive()
    {
        foreach(var square in currentShapeList)
        {
            if (square.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
    public void DeactivateShape()
    {
        if (isShapeActive)
        {
            foreach(var square in currentShapeList)
            {
                square?.GetComponent<ShapeSquare>().DeactivateShape();
            }
        }
        isShapeActive = false;
    }
    private void SetShapeInActive()
    {
        if(!IsOnStartPosition() && IsAnyOfShapeSquareActive())
        {
            foreach(var square in currentShapeList)
            {
                square.gameObject.SetActive(false);
            }
        }
    }
    public void ActivateShape()
    {
        if (!isShapeActive)
        {
            foreach(var square in currentShapeList)
            {
                square?.GetComponent<ShapeSquare>().ActivateShape();
            }
        }
        isShapeActive = true;
    }
    public void RequestNewShape(ShapeData shapeData)
    {
        ResetShapeRectTransform();
        CreateShape(shapeData);
    }

    private void ResetShapeRectTransform()
    {
        shapeRectTransform.anchorMin = startAnchorMin;
        shapeRectTransform.anchorMax = startAnchorMax;
        shapeRectTransform.pivot = startPivot;
        shapeRectTransform.anchoredPosition = startAnchoredPosition;
        this.GetComponent<RectTransform>().localScale = shapeStartScale;
    }

    public void CreateShape(ShapeData shapeData)
    {
        currentShapeData = shapeData;
        TotalSquareNumber = GetNumberOfSquare(shapeData);

        while (currentShapeList.Count < TotalSquareNumber) {
            currentShapeList.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }

        foreach (var square in currentShapeList) { 
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false); 
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var cellSize = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;
        for (var row = 0; row < shapeData.rows; row++) { //set positions to form final shape
            for (var col = 0; col < shapeData.columns; col++) {
                if (shapeData.board[row].column[col])
                {
                    currentShapeList[currentIndexInList].SetActive(true);
                    currentShapeList[currentIndexInList].GetComponent<RectTransform>().localPosition =
                        new Vector2(GetXPositionForShapeSquare(shapeData, col, cellSize), GetYPositionForShapeSquare(shapeData, row, cellSize));

                    currentIndexInList++;
                }
            }
        }
    }

    private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        // Simplified, robust centering: treat center as (rows-1)/2 and offset by row index
        float center = (shapeData.rows - 1) / 2f;
        return (row - center) * moveDistance.y;
    }

    private float GetXPositionForShapeSquare(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        // Simplified, robust centering: treat center as (columns-1)/2 and offset by column index
        float center = (shapeData.columns - 1) / 2f;
        return (column - center) * moveDistance.x;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        shapeRectTransform.anchorMin = new Vector2(0, 0);
        shapeRectTransform.anchorMax = new Vector2(0, 0);
        shapeRectTransform.pivot = new Vector2(0, 0);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, 
        eventData.position, canvas.worldCamera, out Vector2 localPoint);
        shapeRectTransform.localPosition = localPoint + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapePlacedScale;
        GameEvent.CheckIfShapeCanBePlaced?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
         
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
