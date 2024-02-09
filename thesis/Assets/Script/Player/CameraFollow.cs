using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothTime;
    [SerializeField] Transform target;

    Vector3 velocity;

    void Update()
    {
        if (!PlayerManager.Instance.isDead &&
            PlayerManager.Instance.currentState != PlayerManager.Instance.hook &&
            PlayerManager.Instance.currentState != PlayerManager.Instance.endHook)
        {
            Vector3 targetPos = target.position + offset;
            //offset.y = GameManager.Instance.Player.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime); ;
        }
    }
}
