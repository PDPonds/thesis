using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("===== Grid =====")]
    public int gridWidth;
    public int gridHeight;
    public float gridSize;
    public Vector3 gridOrigin;
    [HideInInspector] GridController gridController;



    private void Awake()
    {
        gridController = GetComponent<GridController>();
        gridController.GenerateGrid(gridWidth, gridHeight, gridSize, gridOrigin);
    }

    void Update()
    {

    }
}
