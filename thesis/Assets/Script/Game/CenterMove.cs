using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterMove : MonoBehaviour
{
    private void Update()
    {
        float speed = GameManager.Instance.currentSpeed;
        transform.Translate(Vector2.right * Time.deltaTime * speed);
    }
}
