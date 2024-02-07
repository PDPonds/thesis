using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookState : BaseState
{
    float curOnHookTime;
    public override void EnterState(GameObject go)
    {
        curOnHookTime = PlayerManager.Instance.onHookTime;
        Rigidbody2D rb = PlayerManager.Instance.rb;
        rb.gravityScale = 0;
    }

    public override void UpdateState(GameObject go)
    {
        Transform targetHook = PlayerManager.Instance.curHook;
        Rigidbody2D rb = PlayerManager.Instance.rb;
        curOnHookTime -= Time.deltaTime;
        if (curOnHookTime < 0 || targetHook == null)
        {
            PlayerManager.Instance.curHook = null;
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.endHook);
        }
        Vector3 dir = targetHook.position - PlayerManager.Instance.transform.position;

        rb.AddForce(dir * PlayerManager.Instance.moveToHookSpeed);

    }
}
