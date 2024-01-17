using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGroundAndEnemy : MonoBehaviour
{
    public float moveSpeed;

    private void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

    }
}
