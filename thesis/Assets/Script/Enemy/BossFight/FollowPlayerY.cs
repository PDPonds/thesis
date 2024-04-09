using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerY : MonoBehaviour
{

    void Update()
    {
        transform.position = new Vector3(transform.position.x, PlayerManager.Instance.transform.position.y, 0);
    }
}
