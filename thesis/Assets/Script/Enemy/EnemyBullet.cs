using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
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
                GameObject hitPar = GameManager.Instance.hitParticle;
                GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

                StartCoroutine(playerManager.TakeDamage());

            }
        }
        else if (collision.CompareTag("Ground"))
        {
            GameObject hitPar = GameManager.Instance.hitParticle;
            GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

            Destroy(gameObject);
        }
    }
}
