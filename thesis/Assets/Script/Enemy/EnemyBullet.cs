using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public bool canCounter;
    public bool isCounter;
    public float damage;
    public float speed;

    private void Update()
    {
        if (isCounter)
        {
            transform.Translate(Vector2.right * Time.deltaTime * PlayerManager.Instance.counterBulletSpeed);
        }
        else
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }

        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCounter)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
                {
                    if (!playerManager.noDamage)
                    {
                        StartCoroutine(PlayerTakeDamage(collision, playerManager));
                    }
                }
            }
        }
        else
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.TryGetComponent<IDamageable>(out IDamageable idamageable))
                {
                    StartCoroutine(EnemyTakeDamage(collision, idamageable));

                }
            }

            if (collision.CompareTag("Boss"))
            {
                if (collision.TryGetComponent<BossController>(out BossController idamageable))
                {
                    if (idamageable.curBehavior == BossBehavior.Weakness)
                    {
                        StartCoroutine(EnemyTakeDamage(collision, idamageable));

                    }
                    else
                    {
                        GameObject hitPar = GameManager.Instance.hitParticle;
                        GameManager.Instance.SpawnParticle(hitPar, transform.position);
                        Destroy(gameObject);
                    }
                }
            }

            if (collision.CompareTag("Weakspot"))
            {
                if (collision.TryGetComponent<WeakSpot>(out WeakSpot weakSpot))
                {
                    GameObject hitPar = GameManager.Instance.hitParticle;
                    GameManager.Instance.SpawnParticle(hitPar, collision.transform.position, true);
                    GameManager.Instance.SpawnParticle(GameManager.Instance.slashParticle, transform.position, true);

                    weakSpot.RemoveWeakSpotHP();

                    Destroy(gameObject);
                }
            }
        }

        if (collision.CompareTag("Ground"))
        {
            GameObject hitPar = GameManager.Instance.hitParticle;
            GameManager.Instance.SpawnParticle(hitPar, transform.position);

            Destroy(gameObject);
        }

    }

    IEnumerator PlayerTakeDamage(Collider2D collision, PlayerManager playerManager)
    {
        GameObject hitPar = GameManager.Instance.hitParticle;
        GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);
        yield return StartCoroutine(playerManager.TakeDamage(damage));
        Destroy(gameObject);
    }

    IEnumerator EnemyTakeDamage(Collider2D collision, IDamageable damageable)
    {
        GameObject hitPar = GameManager.Instance.hitParticle;
        GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

        yield return StartCoroutine(damageable.TakeDamage());
        Destroy(gameObject);
    }
}
