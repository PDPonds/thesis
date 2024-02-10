using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBase : MonoBehaviour
{
    Animator animator;

    [SerializeField] float activateDist;
    [SerializeField] bool canUp;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canUp)
        {
            float dist = Vector2.Distance(transform.position, PlayerManager.Instance.transform.position);
            if (dist <= activateDist)
            {
                animator.SetBool("up", true);
            }
        }
    }

}
