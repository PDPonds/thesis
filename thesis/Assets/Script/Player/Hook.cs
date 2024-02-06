using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position =
                Vector3.MoveTowards(transform.position,
                target.position, PlayerManager.Instance.hookSpeed);
        }
    }

}
