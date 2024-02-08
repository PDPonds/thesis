using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndHookState : BaseState
{
    float curTime;
    public override void EnterState(GameObject go)
    {
        curTime = PlayerManager.Instance.waitHookTime;
        PlayerManager.Instance.anim.SetBool("isHookPulling", false);
    }

    public override void UpdateState(GameObject go)
    {
        curTime -= Time.deltaTime;
        if (curTime <= 0)
        {
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.running);
        }
    }
}
