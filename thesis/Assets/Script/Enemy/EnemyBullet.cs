using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;

    private void Update()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
