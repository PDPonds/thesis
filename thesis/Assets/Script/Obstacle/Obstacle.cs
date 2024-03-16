using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float delay;
    public float damage;

    float delayTime;
    bool canHit;

    private void Awake()
    {
        canHit = true;
        delayTime = delay;
    }

    private void Update()
    {
        if (!canHit)
        {
            delayTime -= Time.deltaTime;
            if (delayTime < 0)
            {
                canHit = true;
                delayTime = delay;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canHit)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
                {
                    if (GameManager.Instance.state == GameState.Normal)
                    {
                        if (!playerManager.noDamage)
                        {
                            GameObject hitPar = GameManager.Instance.hitParticle;
                            GameManager.Instance.SpawnParticle(hitPar, other.transform.position);

                            StartCoroutine(playerManager.TakeDamage(damage));
                            canHit = false;
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
                                GameObject hitPar = GameManager.Instance.hitParticle;
                                GameManager.Instance.SpawnParticle(hitPar, other.transform.position);

                                StartCoroutine(playerManager.TakeDamage(damage));
                                canHit = false;
                            }
                        }
                        else
                        {
                            if (!playerManager.noDamage)
                            {
                                GameObject hitPar = GameManager.Instance.hitParticle;
                                GameManager.Instance.SpawnParticle(hitPar, other.transform.position);

                                StartCoroutine(playerManager.TakeDamage(damage));
                                canHit = false;
                            }
                        }
                    }

                }
            }
        }
    }
}
