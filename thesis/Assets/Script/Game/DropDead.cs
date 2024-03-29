using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                playerManager.attackCol.enabled = false;
                playerManager.anim.SetBool("isDead", true);
                SoundManager.Instance.PlayOnShot("Explosive");
                playerManager.onDead?.Invoke();
                playerManager.isDead = true;
            }
        }
    }

}
