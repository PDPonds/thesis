using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int cointAmount;
    [HideInInspector] public bool isDropFormCapsule;
    float delayTime;

    private void Start()
    {
        delayTime = .25f;
    }

    private void Update()
    {
        if (isDropFormCapsule)
        {
            delayTime -= Time.deltaTime;
            if (delayTime < 0)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    PlayerManager.Instance.transform.position, 15 * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerManager>(out PlayerManager manager))
            {
                PlayerManager.Instance.AddCoin(cointAmount);
                GameObject hitPar = GameManager.Instance.getCoinParticle;
                GameManager.Instance.SpawnParticle(hitPar, transform.position);
                Destroy(gameObject);

            }
        }
    }
}
