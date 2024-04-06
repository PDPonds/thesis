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
                if (GameManager.Instance.curBoss == null)
                {
                    GameManager.Instance.SwitchState(GameState.BeforeFirstBoss);
                }
                else
                {
                    GameManager.Instance.SwitchState(GameState.BeforeSecondBoss);
                }
            }
        }
    }
}
