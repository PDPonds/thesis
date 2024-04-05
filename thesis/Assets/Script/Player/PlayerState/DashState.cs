using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BaseState
{
    public override void EnterState(GameObject go)
    {
        PlayerManager.Instance.curDashTime = PlayerManager.Instance.dashTime;
        PlayerManager.Instance.canDash = false;
        CenterMove.instance.transform.position += Vector3.right * PlayerManager.Instance.dashPower;

        GameObject smoke = GameManager.Instance.dashParticle.gameObject;
        GameManager.Instance.SpawnParticle(smoke, go.transform.position, go.transform);
    }

    public override void FixedUpdateState(GameObject go)
    {

    }

    public override void UpdateState(GameObject go)
    {
        PlayerManager.Instance.rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        PlayerManager.Instance.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerManager.Instance.curDashTime -= Time.deltaTime;
        if (PlayerManager.Instance.curDashTime < 0)
        {
            PlayerManager.Instance.rb.constraints = RigidbodyConstraints2D.None;
            PlayerManager.Instance.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            PlayerManager.Instance.SwitchState(PlayerManager.Instance.lastState);
        }
    }
}
