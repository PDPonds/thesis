using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloor : MonoBehaviour
{
    public Transform mesh;

    private void Awake()
    {
        onSpawn();
    }

    public void onSpawn()
    {
        float min = GameManager.Instance.minFloorSize;
        float max = GameManager.Instance.maxFloorSize;

        float size = Random.Range(min, max);

        transform.localScale = new Vector2(size, 1);

    }

}
