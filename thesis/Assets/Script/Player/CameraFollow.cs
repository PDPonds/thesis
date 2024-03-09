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
    //[SerializeField] float bossYCamBeforeSpawn;
    [SerializeField] float bossYCam;

    Vector3 velocity;

    void Update()
    {
        if (!PlayerManager.Instance.isDead)
        {
            Vector3 targetPos = target.position + offset;
            if (GameManager.Instance.state == GameState.Normal)
            {
                if (targetPos.y > maxY) targetPos.y = maxY;
                if (targetPos.y < minY) targetPos.y = minY;
            }
            else if (GameManager.Instance.state == GameState.BossFight)
            {
                if(GameManager.Instance.curBoss != null &&
                    GameManager.Instance.curBoss.activeSelf)
                    targetPos.y = bossYCam;
                else
                {
                    if (targetPos.y > maxY) targetPos.y = maxY;
                    if (targetPos.y < minY) targetPos.y = minY;
                }
            }

            //offset.y = GameManager.Instance.Player.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime); ;
        }
    }
}
