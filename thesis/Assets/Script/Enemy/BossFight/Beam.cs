using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
            {
                if (GameManager.Instance.state == GameState.Normal)
                {
                    if (!playerManager.noDamage)
                    {
                        StartCoroutine(PlayerTakeDamage(collision, playerManager));
                    }
                }
                else if (GameManager.Instance.state == GameState.BossFight)
                {
                    if (GameManager.Instance.curBoss != null &&
                        GameManager.Instance.curBoss.activeSelf)
                    {
                        BossController boss = GameManager.Instance.curBoss.GetComponent<BossController>();
                        if (!playerManager.noDamage && boss.curBehavior != BossBehavior.Escape)
                        {
                            StartCoroutine(PlayerTakeDamage(collision, playerManager));
                        }
                    }
                    else
                    {
                        if (!playerManager.noDamage)
                        {
                            StartCoroutine(PlayerTakeDamage(collision, playerManager));
                        }
                    }
                }
            }
        }
    }

    IEnumerator PlayerTakeDamage(Collider2D collision, PlayerManager playerManager)
    {
        //if (isMissile) SoundManager.Instance.PlayOnShot("Explosive");
        //else SoundManager.Instance.PlayOnShot("LaserHit");

        yield return StartCoroutine(playerManager.TakeDamage(damage));
        //Destroy(gameObject);
    }

}
