using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDead : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
            {
                playerManager.isDropDead = true;
                playerManager.Die();
            }
        }
    }
}
