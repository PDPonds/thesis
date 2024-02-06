using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public Transform target;
    public LineRenderer line;

    private void Start()
    {
        line.positionCount = 2;
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

        line.SetPosition(0, transform.position);
        line.SetPosition(1, PlayerManager.Instance.transform.position);

    }

}
