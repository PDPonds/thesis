using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BaseState
{
    public override void EnterState(GameObject go)
    {
        PlayerManager.Instance.canDash = false;
        CenterMove.instance.transform.position += Vector3.right * PlayerManager.Instance.dashPower;

        PlayerManager.Instance.rb.simulated = false;

        GameObject smoke = GameManager.Instance.dashParticle.gameObject;
        GameManager.Instance.SpawnParticle(smoke, go.transform.position, go.transform);
    }

    public override void FixedUpdateState(GameObject go)
    {

    }

    public override void UpdateState(GameObject go)
    {
        PlayerManager.Instance.curDashDelay -= Time.deltaTime;
        if (PlayerManager.Instance.curDashDelay < 0)
        {
            PlayerManager.Instance.rb.simulated = true;
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.running);
        }
    }
}
