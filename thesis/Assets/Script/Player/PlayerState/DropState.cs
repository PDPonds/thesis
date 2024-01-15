using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropState : BaseState
{
    public override void EnterState(GameObject obj)
    {
        PlayerManager.Instance.SetupJointSpringWeight(0, 0, 0, 0);
        PlayerManager.Instance.pac.enabled = false;
    }

    public override void UpdateState(GameObject obj)
    {

    }
}
