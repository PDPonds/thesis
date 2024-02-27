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


            }
        }

        if (collision.CompareTag("Capsule"))
        {
            if (collision.TryGetComponent<CoinCapsule>(out CoinCapsule cc))
            {
                CoinCapsule capsule = collision.GetComponent<CoinCapsule>();
                GameManager.Instance.SpawnCoin(collision.transform.position, capsule.dropCoinAmount);
            }

            if (collision.TryGetComponent<ShurikenCapsule>(out ShurikenCapsule shurikenCapsule))
            {
                SpecialGadget gadget = shurikenCapsule.gadget;
                int amount = shurikenCapsule.amount;
                PlayerManager.Instance.AddGadget(gadget, amount);
            }

            float time = GameManager.Instance.shakeDuration;
            float mag = GameManager.Instance.shakeMagnitude;
            StartCoroutine(GameManager.Instance.SceneShake(time, mag));

            GameObject hitPar = GameManager.Instance.hitParticle;
            GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

            if (!PlayerManager.Instance.onGrounded)
            {
                PlayerManager.Instance.JumpAfterAttack(PlayerManager.Instance.jumpForce /** 0.75f*/);
            }

            Destroy(collision.transform.parent.gameObject);
            PlayerManager.Instance.attackCol.enabled = false;
        }

    }

}
