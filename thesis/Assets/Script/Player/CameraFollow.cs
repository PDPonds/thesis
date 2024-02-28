using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("===== Normal =====")]
    [SerializeField] float maxY;
    [SerializeField] float minY;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothTime;
    [SerializeField] Transform target;
    [Header("===== Boss =====")]
    [SerializeField] float bossYCam;

    Vector3 velocity;

    void Update()
    {
        if (!PlayerManager.Instance.isDead &&
            PlayerManager.Instance.currentState != PlayerManager.Instance.hook &&
            PlayerManager.Instance.currentState != PlayerManager.Instance.endHook)
        {
            Vector3 targetPos = target.position + offset;
            if (GameManager.Instance.state == GameState.Normal)
            {
                if (targetPos.y > maxY) targetPos.y = maxY;
                if (targetPos.y < minY) targetPos.y = minY;
            }
            else if(GameManager.Instance.state == GameState.BossFight)
            {
                targetPos.y = bossYCam;
            }

            //offset.y = GameManager.Instance.Player.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime); ;
        }
    }
}
