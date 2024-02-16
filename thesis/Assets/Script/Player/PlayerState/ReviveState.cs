using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveState : BaseState
{
    float curReviveTime;
    public override void EnterState(GameObject go)
    {
        PlayerManager.Instance.currentHp = PlayerManager.Instance.maxHp;
        PlayerManager.Instance.isDead = false;
        PlayerManager.Instance.anim.SetBool("isDead", false);
        PlayerManager.Instance.noDamage = true;
        PlayerManager.reviveItemCount--;
        curReviveTime = PlayerManager.Instance.reviveTime;
    }

    public override void FixedUpdateState(GameObject go)
    {
        curReviveTime -= Time.deltaTime;
        if (curReviveTime < 0)
        {
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.running);
        }
    }

    public override void UpdateState(GameObject go)
    {

    }
}
