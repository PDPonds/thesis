using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float delayTime;
    [SerializeField] float damageRange;
    [SerializeField] Transform triggerTransform;
    [SerializeField] LayerMask playerMask;

    Animator triggerAnim;

    float curDelayTime;
    bool isTrigger;

    private void Awake()
    {
        triggerAnim = triggerTransform.GetComponent<Animator>();
    }


    private void Update()
    {
        if (isTrigger)
        {
            curDelayTime -= Time.deltaTime;
            if (curDelayTime < 0)
            {
                //Play Explosive Sound
                GameObject mineParticle = GameManager.Instance.mineExplosion;
                GameManager.Instance.SpawnParticle(mineParticle, transform.position, new Vector3(-90, 0, 0));

                Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, damageRange, playerMask);
                if (player.Length > 0)
                {
                    StartCoroutine(PlayerManager.Instance.TakeDamage(damage));
                }

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerManager>(out PlayerManager manager))
            {
                //Play Trigger Sound
                triggerAnim.Play("MineTrigger");
                curDelayTime = delayTime;
                isTrigger = true;

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }

}
