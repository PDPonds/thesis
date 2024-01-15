using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAirState : BaseState
{
    public override void EnterState(GameObject obj)
    {
        PlayerManager.Instance.pac.enabled = false;
    }

    public override void UpdateState(GameObject obj)
    {
        
    }

}
