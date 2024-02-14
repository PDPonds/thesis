using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBlade : MonoBehaviour
{
    public float delay;
    public float damage;

    float delayTime;
    bool canHit;

    private void Start()
    {
        canHit = true;
        delayTime = delay;
    }

    void Update()
    {
        if (!canHit)
        {
            delayTime -= Time.deltaTime;
            if (delayTime < 0)
            {
                canHit = true;
                delayTime = delay;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canHit)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent<PlayerManager>(out PlayerManager manager))
                {
                    GameObject hitPar = GameManager.Instance.hitParticle;
                    GameManager.Instance.SpawnParticle(hitPar, other.transform.position);

                    StartCoroutine(manager.TakeDamage(damage));
                    canHit = false;
                }
            }
        }
    }
}
