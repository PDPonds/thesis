using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : BaseState
{
    public override void EnterState(GameObject go)
    {
        Animator anim = PlayerManager.Instance.playerAnimation.anim;
        anim.SetBool("onAir", false);
        anim.SetBool("Slide", false);
    }

    public override void UpdateState(GameObject go)
    {
        if (PlayerManager.Instance.onGrounded) PlayerManager.Instance.jumpCount = 1;

    }


}
