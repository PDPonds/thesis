using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : BaseState
{
    //float currentSlideTime;
    public override void EnterState(GameObject go)
    {
        Animator anim = PlayerManager.Instance.anim;
        anim.SetBool("onAir", false);
        anim.SetBool("Slide", true);

        PlayerManager.Instance.rb.velocity = Vector2.down * PlayerManager.Instance.slideDrag;

        Vector2 size = PlayerManager.Instance.slideCol;
        Vector2 offset = PlayerManager.Instance.slideColPos;
        PlayerManager.Instance.SetupPlayerCol(size, offset, CapsuleDirection2D.Horizontal);

        //currentSlideTime = PlayerManager.Instance.slideTime;
        Rigidbody2D rb = PlayerManager.Instance.rb;
        rb.gravityScale = 3;
    }

    public override void FixedUpdateState(GameObject go)
    {

    }

    public override void UpdateState(GameObject go)
    {
        PlayerManager.Instance.hasGroundOnHead = Physics2D.Raycast(PlayerManager.Instance.transform.position,
            PlayerManager.Instance.transform.up, PlayerManager.Instance.checkHeadRange);

        if (!PlayerManager.Instance.isSlideButtonDown && !PlayerManager.Instance.hasGroundOnHead)
        {
            PlayerManager.Instance.isSlideButtonDown = false;
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.running);
        }
    }

}
