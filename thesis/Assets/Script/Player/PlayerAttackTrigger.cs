using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PlayerManager.Instance.augmentManager.HasSkill(1, out int counterSkillIndex))
        {
            AugmentSlot counterSkillSlot = PlayerManager.Instance.augmentManager.skillInventory[counterSkillIndex];
            if (counterSkillSlot.ready)
            {
                if (collision.CompareTag("EnemyBullet"))
                {
                    SkillSO skill = counterSkillSlot.skill;
                    int level = counterSkillSlot.level;
                    float delayTime = skill.skillLevelAndDelays[level - 1].delay;

                    SpriteRenderer spriteRenderer = collision.GetComponent<SpriteRenderer>();
                    spriteRenderer.flipX = false;

                    Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.zero;
                    rb.AddForce(Vector2.right * GameManager.Instance.currentSpeed * 2f, ForceMode2D.Impulse);

                    EnemyBullet enemyBullet = collision.GetComponent<EnemyBullet>();
                    enemyBullet.isCounter = true;

                    counterSkillSlot.delay = delayTime;
                    counterSkillSlot.ready = false;
                }

            }
        }

        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<IDamageable>(out IDamageable idamageable))
            {
                GameObject hitPar = GameManager.Instance.hitParticle;
                GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

                StartCoroutine(idamageable.TakeDamage());
            }
        }
    }

}
