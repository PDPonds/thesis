using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGround : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    private void Awake()
    {
        float x = GameManager.Instance.Player.position.x + offset.x;
        float z = GameManager.Instance.Player.position.z + offset.z;
        Vector3 pos = new Vector3(x, 0, z);
        transform.position = pos;
    }

    private void Update()
    {
        float x = GameManager.Instance.Player.position.x + offset.x;
        float z = GameManager.Instance.Player.position.z + offset.z;
        Vector3 pos = new Vector3(x, 0, z);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Destroy(collision.gameObject);

            Transform spawnPos = GameManager.Instance.SpawnGround;
            SpawnFloor spawnFloor = spawnPos.GetComponent<SpawnFloor>();
            spawnFloor.GenerateFloor();
        }
    }
}
