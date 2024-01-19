using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerManager>(out PlayerManager manager))
            {
                if(PlayerManager.Instance.Heal())
                {
                    GameObject hitPar = GameManager.Instance.healParticle;
                    GameManager.Instance.SpawnParticle(hitPar, other.transform.position);

                    Destroy(gameObject);
                }
                
            }
        }
    }
}
