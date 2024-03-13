using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float force;
    public float damage;
    Rigidbody2D rb;

    [HideInInspector] public float upForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            GameObject hitPar = GameManager.Instance.hitParticle;
            GameManager.Instance.SpawnParticle(hitPar, transform.position);

            Vector2 dir = new Vector2(upForce - 1, upForce);
            rb.AddForce(dir * force, ForceMode2D.Impulse);

        }

    }

    IEnumerator PlayerTakeDamage(Collider2D collision, PlayerManager playerManager)
    {
        //if (isMissile) SoundManager.Instance.PlayOnShot("Explosive");
        //else SoundManager.Instance.PlayOnShot("LaserHit");

        yield return StartCoroutine(playerManager.TakeDamage(damage));
        Destroy(gameObject);
    }


}
