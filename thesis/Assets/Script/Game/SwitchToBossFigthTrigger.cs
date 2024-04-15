using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToBossFigthTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!GameManager.Instance.isBossClear)
            {
                if (GameManager.Instance.curBoss != null)
                {
                    BossController boss = GameManager.Instance.curBoss.GetComponent<BossController>();
                    if (!boss.isEnterHalfHP)
                    {
                        GameManager.Instance.SwitchState(GameState.BeforeFirstBoss);
                    }
                    else
                    {
                        GameManager.Instance.SwitchState(GameState.BeforeSecondBoss);
                    }
                }
                else
                {
                    GameManager.Instance.SwitchState(GameState.BeforeFirstBoss);
                }
            }
        }
    }
}
