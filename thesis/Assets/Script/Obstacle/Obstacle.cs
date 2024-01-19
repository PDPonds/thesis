using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float delay;

    float delayTime;
    bool canHit;

    private void Awake()
    {
        canHit = true;
        delayTime = delay;
    }

    private void Update()
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

                    StartCoroutine(manager.TakeDamage());
                    canHit = false;
                }
            }
        }
    }
}
