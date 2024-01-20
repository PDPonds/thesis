using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private void Update()
    {
        Destroy(gameObject, 5f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<IDamageable>(out IDamageable idamageable))
            {
                StartCoroutine(EnemyTakeDamage(collision, idamageable));
            }
        }

        if (collision.CompareTag("Ground"))
        {
            GameObject hitPar = GameManager.Instance.hitParticle;
            GameManager.Instance.SpawnParticle(hitPar, transform.position);

            Destroy(gameObject);
        }
    }
    IEnumerator EnemyTakeDamage(Collider2D collision, IDamageable damageable)
    {
        GameObject hitPar = GameManager.Instance.hitParticle;
        GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

        StartCoroutine(damageable.TakeDamage());
        yield return new WaitForSecondsRealtime(0.2f);
        Destroy(gameObject);
    }
}
