using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceState : BaseState
{
    public override void EnterState(GameObject obj)
    {
        float rootWeight = PlayerManager.Instance.hardSpring;
        float armWeight = PlayerManager.Instance.softSpring;
        float legWeight = PlayerManager.Instance.hardSpring;
        float bodyWeight = PlayerManager.Instance.hardSpring;
        PlayerManager.Instance.SetupJointSpringWeight(rootWeight, bodyWeight, armWeight, legWeight);
        PlayerManager.Instance.pac.enabled = true;

    }

    public override void UpdateState(GameObject obj)
    {

    }
}
