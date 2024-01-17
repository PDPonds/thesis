using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAirState : BaseState
{
    public override void EnterState(GameObject go)
    {
        Animator anim = PlayerManager.Instance.anim;
        anim.SetBool("onAir", true);
        anim.SetBool("Slide", false);

        Vector2 size = PlayerManager.Instance.runningCol;
        Vector2 offset = PlayerManager.Instance.runningColPos;
        PlayerManager.Instance.SetupPlayerCol(size, offset);

        PlayerManager.Instance.rb.velocity = Vector2.up * PlayerManager.Instance.jumpForce;
    }

    public override void UpdateState(GameObject go)
    {
        if (PlayerManager.Instance.onGrounded)
        {
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.running);
        }
    }


}
