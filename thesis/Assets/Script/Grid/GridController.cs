using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [HideInInspector] public Grid grid;

    public void GenerateGrid(int x, int z, float cellSize, Vector3 origin)
    {
        grid = new Grid(x, z, cellSize, origin);

    }

}
