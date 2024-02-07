using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<IDamageable>(out IDamageable idamageable))
            {
                if (PlayerManager.Instance.curHook != null)
                {
                    Hook hook = PlayerManager.Instance.curHook.GetComponent<Hook>();
                    hook.DestroyHook();
                }

                GameObject hitPar = GameManager.Instance.hitParticle;
                GameManager.Instance.SpawnParticle(hitPar, collision.transform.position);

                StartCoroutine(idamageable.TakeDamage());

                if (!PlayerManager.Instance.onGrounded)
                {
                    PlayerManager.Instance.JumpAfterAttack(PlayerManager.Instance.jumpForce * 0.75f);
                }

                PlayerManager.Instance.attackCol.enabled = false;
            }
        }
    }

}
