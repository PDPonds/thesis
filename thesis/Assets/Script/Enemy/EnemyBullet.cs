using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public bool canCounter;
    public bool isCounter;
    public float damage;
    public float speed;
    public bool isMissile;

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
                    StartCoroutine(EnemyTakeDamage(collision, idamageable));

                }
            }

            if (collision.CompareTag("Weakspot"))
            {
                if (collision.TryGetComponent<WeakSpot>(out WeakSpot weakSpot))
                {
                    StartCoroutine(EnemyTakeDamage(collision, weakSpot.bossController));
                    weakSpot.RemoveWeakSpotHP();
                    GameManager.Instance.SpawnParticle(GameManager.Instance.weakspotParticle, transform.position, true);

                    Destroy(gameObject);
                }
            }
        }

        if (collision.CompareTag("Ground"))
        {
            GameObject hitPar = GameManager.Instance.hitParticle;
            GameManager.Instance.SpawnParticle(hitPar, transform.position);
            SoundManager.Instance.PlayOnShot("LaserHit");
            Destroy(gameObject);
        }

    }

    IEnumerator PlayerTakeDamage(Collider2D collision, PlayerManager playerManager)
    {
        if (isMissile) SoundManager.Instance.PlayOnShot("Explosive");
        else SoundManager.Instance.PlayOnShot("LaserHit");

        yield return StartCoroutine(playerManager.TakeDamage(damage));
        Destroy(gameObject);
    }

    IEnumerator EnemyTakeDamage(Collider2D collision, IDamageable damageable)
    {
        SoundManager.Instance.PlayOnShot("LaserHit");
        yield return StartCoroutine(damageable.TakeDamage());
        Destroy(gameObject);
    }
}
