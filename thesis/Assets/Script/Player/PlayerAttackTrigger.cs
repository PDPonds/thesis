using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            if (collision.TryGetComponent<IDamageable>(out IDamageable idamageable))
            {

                //if (PlayerManager.Instance.curHook != null)
                //{
                //    PlayerManager.Instance.JumpAfterAttack(PlayerManager.Instance.jumpForce * 0.75f);

                //    Hook hook = PlayerManager.Instance.curHook.GetComponent<Hook>();
                //    hook.DestroyHook();
                //    PlayerManager.Instance.curHook = null;
                //}

                float healAmount = PlayerManager.Instance.maxHp * PlayerManager.Instance.stealHPLevels[PlayerManager.upgradeStealHpLevel] / 100f;
                PlayerManager.Instance.Heal(healAmount);

                //GameObject hitPar = GameManager.Instance.hitParticle;
                //GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

                StartCoroutine(idamageable.TakeDamage());

                if (!PlayerManager.Instance.onGrounded)
                {
                    PlayerManager.Instance.JumpAfterAttack(PlayerManager.Instance.jumpForce /** 0.75f*/);
                }

                PlayerManager.Instance.attackCol.enabled = false;

                if (PlayerManager.Instance.noDamage)
                {
                    PlayerManager.Instance.curNoDamageTime = PlayerManager.Instance.noDamageTime;
                    Physics2D.IgnoreLayerCollision(3, 7, false);
                    PlayerManager.Instance.noDamage = false;
                }
            }
        }

        if (collision.CompareTag("EnemyBullet"))
        {
            if (collision.transform.TryGetComponent<EnemyBullet>(out EnemyBullet eBullet))
            {
                if (eBullet.canCounter)
                {
                    eBullet.isCounter = true;
                    SpriteRenderer sprite = eBullet.transform.GetComponent<SpriteRenderer>();
                    sprite.flipX = true;

                    CapsuleCollider2D capCol = eBullet.transform.GetComponent<CapsuleCollider2D>();
                    Vector2 curOffset = capCol.offset;
                    curOffset.x = capCol.offset.x * -1f;
                    capCol.offset = curOffset;
                    SoundManager.Instance.PlayOnShot("LaserShot");

                    if (PlayerManager.Instance.noDamage)
                    {
                        PlayerManager.Instance.curNoDamageTime = PlayerManager.Instance.noDamageTime;
                        Physics2D.IgnoreLayerCollision(3, 7, false);
                        PlayerManager.Instance.noDamage = false;
                    }

                }
            }
        }

        if (collision.CompareTag("Capsule"))
        {
            if (collision.TryGetComponent<CoinCapsule>(out CoinCapsule cc))
            {
                CoinCapsule capsule = collision.GetComponent<CoinCapsule>();
                GameManager.Instance.SpawnCoin(collision.transform.position, capsule.dropCoinAmount);
            }

            if (collision.TryGetComponent<GadgetCapsule>(out GadgetCapsule gadgetCapsule))
            {
                SpecialGadget gadget = gadgetCapsule.gadget;
                int amount = gadgetCapsule.amount;
                PlayerManager.Instance.AddGadget(gadget, amount);
            }

            float time = GameManager.Instance.shakeDuration;
            float mag = GameManager.Instance.shakeMagnitude;
            StartCoroutine(GameManager.Instance.SceneShake(time, mag));

            GameObject hitPar = GameManager.Instance.hitParticle;
            GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

            SoundManager.Instance.PlayOnShot("SlashItem");

            if (!PlayerManager.Instance.onGrounded)
            {
                PlayerManager.Instance.JumpAfterAttack(PlayerManager.Instance.jumpForce /** 0.75f*/);
            }

            if (PlayerManager.Instance.noDamage)
            {
                PlayerManager.Instance.curNoDamageTime = PlayerManager.Instance.noDamageTime;
                Physics2D.IgnoreLayerCollision(3, 7, false);
                PlayerManager.Instance.noDamage = false;
            }

            Destroy(collision.transform.parent.gameObject);
            PlayerManager.Instance.attackCol.enabled = false;
        }

    }

}
