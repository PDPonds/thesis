using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : BaseState
{
    public override void EnterState(GameObject go)
    {
        Animator anim = PlayerManager.Instance.anim;
        anim.Play("Hurt");
        anim.SetBool("Slide", false);
        Rigidbody2D rb = PlayerManager.Instance.rb;
        rb.gravityScale = 3;
    }

    public override void UpdateState(GameObject go)
    {
        Animator anim = PlayerManager.Instance.anim;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                PlayerManager.Instance.SwitchState(PlayerManager.Instance.running);
            }
        }
        else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.running);
        }
    }
}
