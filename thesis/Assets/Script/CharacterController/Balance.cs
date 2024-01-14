using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] Transform root;
    void Update()
    {
        transform.position = root.position;
    }
}
