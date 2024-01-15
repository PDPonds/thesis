using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    int width;
    int height;
    float cellSize;
    Vector3 originPos;

    Cell[,] cellArray;

    public Grid(int width, int height, float cellSize, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;

        cellArray = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                cellArray[x, z] = new Cell();

                if (x < width / 2)
                {
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.red, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.red, 100f);
                }

            }

        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width / 2, height), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(width / 2, 0), GetWorldPosition(width / 2, height), Color.red, 100f);

    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPos;
    }

    public void GetXYFormWorldPos(Vector3 worldPos, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPos - originPos).x / cellSize);
        z = Mathf.FloorToInt((worldPos - originPos).z / cellSize);

    }

    public Vector3 GetCenterOnCell(int x, int z)
    {
        return GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * 0.5f;
    }

    public bool GetCellIsEmpty(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return cellArray[x, z].hasCharacter;
        }
        return false;
    }

    public void SetupFloor(int x, int z, GameObject floor)
    {
        cellArray[x, z].floorObj = floor;
    }

}

[Serializable]
public class Cell
{
    public GameObject floorObj;
    public bool hasCharacter;
}
