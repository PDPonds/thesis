using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGround : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    private void Awake()
    {
        float x = GameManager.Instance.Camera.position.x + offset.x;
        Vector3 pos = new Vector3(x, 0, 0);
        transform.position = pos;
    }

    private void Update()
    {
        float x = GameManager.Instance.Camera.position.x + offset.x;
        Vector3 pos = new Vector3(x, 0, 0);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DestroyGround"))
        {
            Destroy(collision.gameObject);

            GameManager.Instance.GenerateFloor();
        }

        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }

    }
}
