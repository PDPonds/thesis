using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : BaseState
{
    public override void EnterState(GameObject go)
    {
        Animator anim = PlayerManager.Instance.anim;
        anim.SetBool("onAir", false);
        anim.SetBool("Slide", false);

        Vector2 size = PlayerManager.Instance.runningCol;
        Vector2 offset = PlayerManager.Instance.runningColPos;
        PlayerManager.Instance.SetupPlayerCol(size, offset, CapsuleDirection2D.Vertical);

        Rigidbody2D rb = PlayerManager.Instance.rb;
        //rb.velocity = Vector3.zero;
        rb.gravityScale = 3;
    }

    public override void FixedUpdateState(GameObject go)
    {
     
    }

    public override void UpdateState(GameObject go)
    {
        //if (PlayerManager.Instance.onGrounded) PlayerManager.Instance.jumpCount = 2;

    }


}
