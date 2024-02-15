using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterMove : MonoBehaviour
{
    private void Update()
    {
        float speed = GameManager.Instance.currentSpeed;

        if (PlayerManager.Instance.currentState != PlayerManager.Instance.hook)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);

            transform.position = new Vector3(transform.position.x,
                PlayerManager.Instance.transform.position.y, transform.position.z);
        }
        else if (PlayerManager.Instance.currentState == PlayerManager.Instance.hook)
        {
            transform.position = PlayerManager.Instance.transform.position;
        }
        //else
        //{
        //    //float speed = GameManager.Instance.currentSpeed;
        //    transform.Translate(Vector2.right * CenterMoveHookSpeed);

        //    transform.position = new Vector3(transform.position.x,
        //        PlayerManager.Instance.transform.position.y, transform.position.z);
        //}
    }
}
