using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWindow : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            anim.Play("Window Break");
        }
    }
}
