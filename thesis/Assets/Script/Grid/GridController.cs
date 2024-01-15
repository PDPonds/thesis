using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [HideInInspector] public Grid grid;

    public void GenerateGrid(int width, int height, float cellSize, Vector3 origin)
    {
        grid = new Grid(width, height, cellSize, origin);
    }

    public void GenerateFloor(int width, int height)
    {
        GameObject floorObj = GameManager.Instance.floorPrefabs;
        Transform parent = GameManager.Instance.mapParent;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 worldPos = grid.GetCenterOnCell(x, z);
                GameObject currentFloor = Instantiate(floorObj, worldPos + new Vector3(0f, -.5f, 0f), Quaternion.identity);
                currentFloor.transform.SetParent(parent);
                grid.SetupFloor(x, z, currentFloor);
            }
        }
    }

}
