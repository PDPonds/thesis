using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDragState : BaseState
{
    public override void EnterState(GameObject obj)
    {
        float rootWeight = PlayerManager.Instance.softSpring;
        float armWeight = PlayerManager.Instance.softSpring;
        float legWeight = PlayerManager.Instance.softSpring;
        float bodyWeight = PlayerManager.Instance.softSpring;
        PlayerManager.Instance.SetupJointSpringWeight(rootWeight, bodyWeight, armWeight, legWeight);
        PlayerManager.Instance.pac.enabled = false;
    }

    public override void UpdateState(GameObject obj)
    {
        
    }
}
