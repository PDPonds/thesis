using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public void EnableAttack()
    {
        PlayerManager.Instance.attackCol.enabled = true;
    }

    public void DisableAttackCol()
    {
        PlayerManager.Instance.attackCol.enabled = false;
    }

}
