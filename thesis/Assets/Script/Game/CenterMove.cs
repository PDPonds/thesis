using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterMove : MonoBehaviour
{
    private void Update()
    {
        if (PlayerManager.Instance.currentState != PlayerManager.Instance.hook)
        {
            float speed = GameManager.Instance.currentSpeed;
            transform.Translate(Vector2.right * Time.deltaTime * speed);

            transform.position = new Vector3(transform.position.x,
                PlayerManager.Instance.transform.position.y, transform.position.z);
        }
    }
}
