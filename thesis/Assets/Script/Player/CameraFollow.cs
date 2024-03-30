using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    [Header("===== Normal =====")]
    [SerializeField] float maxY;
    [SerializeField] float minY;

    [SerializeField] float normalX;
    //[SerializeField] float minX;
    //[SerializeField] float maxX;

    [SerializeField] Vector3 offset;
    [SerializeField] float smoothTime;
    [SerializeField] Transform target;

    [Header("===== Boss =====")]
    //[SerializeField] float bossYCamBeforeSpawn;
    [SerializeField] float FirstStateBossYCam;
    [SerializeField] float SecondStateBossYCam;
    /*[HideInInspector] public float moveLeftInput;*/
    /*[HideInInspector] public float moveRightInput;*/

    Vector3 velocity;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!PlayerManager.Instance.isDead)
        {
            Vector3 targetPos = target.position + offset;
            if (GameManager.Instance.state == GameState.Normal)
            {
                if (targetPos.y > maxY) targetPos.y = maxY;
                if (targetPos.y < minY) targetPos.y = minY;
                targetPos.x = target.position.x + normalX;
            }
            else if (GameManager.Instance.state == GameState.BossFight)
            {
                if (GameManager.Instance.curBoss != null &&
                    GameManager.Instance.curBoss.activeSelf)
                {
                    BossController boss = GameManager.Instance.curBoss.GetComponent<BossController>();
                    if (boss.isEnterHalfHP)
                    {
                        targetPos.y = SecondStateBossYCam;
                    }
                    else
                    {
                        targetPos.y = FirstStateBossYCam;
                    }
                    //float moveInput = moveLeftInput - moveRightInput;
                    //float speed = moveInput * GameManager.Instance.currentSpeed /** Time.deltaTime*/;
                    //targetPos.x += speed;
                    //if (targetPos.x < target.position.x + minX) targetPos.x = target.position.x + minX;
                    //if (targetPos.x > target.position.x + maxX) targetPos.x = target.position.x + maxX;
                }
                else
                {
                    if (targetPos.y > maxY) targetPos.y = maxY;
                    if (targetPos.y < minY) targetPos.y = minY;
                    targetPos.x = target.position.x + normalX; ;
                }
            }

            //offset.y = GameManager.Instance.Player.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime); ;
        }
    }
}
