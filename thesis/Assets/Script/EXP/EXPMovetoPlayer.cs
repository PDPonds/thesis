using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPObj : MonoBehaviour
{
    public float expAmount;
    void Update()
    {
        float speed = GameManager.Instance.currentSpeed * 2f;
        transform.position = Vector3.MoveTowards(transform.position,
            GameManager.Instance.Player.position,
            Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerManager>(out PlayerManager player))
            {
                player.AddExp(expAmount);
                Destroy(gameObject);
            }
        }
    }


}
