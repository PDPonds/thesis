using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAirState : BaseState
{
    public override void EnterState(GameObject go)
    {
        Animator anim = PlayerManager.Instance.playerAnimation.anim;
        anim.SetBool("onAir", true);
        anim.SetBool("Slide", false);

        PlayerManager.Instance.rb.velocity = Vector2.up * PlayerManager.Instance.jumpForce;
    }

    public override void UpdateState(GameObject go)
    {
        if (PlayerManager.Instance.onGrounded)
        {
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.runningState);
        }
    }


}
