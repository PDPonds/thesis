using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float maxY;
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
            if (targetPos.y > maxY)
            {
                targetPos.y = maxY;
            }
            //offset.y = GameManager.Instance.Player.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime); ;
        }
    }
}
