using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIcon : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("canDash", PlayerManager.Instance.canDash);
    }

    public void PlayDashReady()
    {
        anim.Play("Dash Ready");
    }

}
