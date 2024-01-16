using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : BaseState
{
    float currentSlideTime;
    public override void EnterState(GameObject go)
    {
        Animator anim = PlayerManager.Instance.playerAnimation.anim;
        anim.SetBool("onAir", false);
        anim.SetBool("Slide", true);

        PlayerManager.Instance.rb.velocity = Vector2.down * PlayerManager.Instance.slideDrag;

        currentSlideTime = PlayerManager.Instance.slideTime;
    }

    public override void UpdateState(GameObject go)
    {
        currentSlideTime -= Time.deltaTime;
        if (currentSlideTime < 0)
        {
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.runningState);
        }
    }

}
