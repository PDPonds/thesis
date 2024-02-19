using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookState : BaseState
{
    float curOnHookTime;
    public override void EnterState(GameObject go)
    {
        //PlayerManager.Instance.anim.SetBool("isHookPulling", true);
        //curOnHookTime = PlayerManager.Instance.onHookTime;
        //Rigidbody2D rb = PlayerManager.Instance.rb;
        //rb.gravityScale = 0;
    }

    public override void FixedUpdateState(GameObject go)
    {
        //Transform targetHook = PlayerManager.Instance.curHook;
        //Rigidbody2D rb = PlayerManager.Instance.rb;
        
        
        
        //curOnHookTime -= Time.deltaTime;

        //if (curOnHookTime < 0)
        //{
        //    PlayerManager.Instance.SwitchState(PlayerManager.Instance.endHook);
        //    PlayerManager.Instance.curHook = null;
        //}


        //if (targetHook != null)
        //{
        //    Vector3 dir = targetHook.position - PlayerManager.Instance.transform.position;
        //    dir = dir.normalized;
        //    rb.AddForce(dir * PlayerManager.Instance.moveToHookSpeed);
        //    //rb.velocity = dir * PlayerManager.Instance.moveToHookSpeed;
        //    if (Vector3.Distance(PlayerManager.Instance.transform.position,
        //        targetHook.position) < 0.1f)
        //    {
        //        PlayerManager.Instance.curHook = null;
        //        PlayerManager.Instance.SwitchState(PlayerManager.Instance.endHook);
        //    }
        //}
        //else
        //{
        //    //rb.gravityScale = 3;
        //    //rb.velocity = Vector3.zero;
        //    PlayerManager.Instance.SwitchState(PlayerManager.Instance.endHook);
        //}
    }

    public override void UpdateState(GameObject go)
    {
        
    }
}
