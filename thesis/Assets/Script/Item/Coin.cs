using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int cointAmount;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerManager>(out PlayerManager manager))
            {
                PlayerManager.Instance.AddCoin(cointAmount);
                GameObject hitPar = GameManager.Instance.getCoinParticle;
                GameManager.Instance.SpawnParticle(hitPar, other.transform.position);
                Destroy(gameObject);

            }
        }
    }
}
