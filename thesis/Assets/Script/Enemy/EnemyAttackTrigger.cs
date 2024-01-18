using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
            {
                GameObject hitPar = GameManager.Instance.hitParticle;
                GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

                playerManager.TakeDamage();
            }
        }
    }
}
