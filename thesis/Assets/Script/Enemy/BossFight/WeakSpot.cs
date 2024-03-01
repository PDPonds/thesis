using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    public BossController bossController;
    public int weakSpotHP;
    public Transform curPos;

    private void Start()
    {
        ResetWeakSpotHP();
    }

    public void ResetWeakSpotHP()
    {
        weakSpotHP = bossController.bossSO.weakSpotHP;
        bossController.SwitchWeakSpotPos();
    }

    public void RemoveWeakSpotHP()
    {
        if (bossController.curBehavior == BossBehavior.Normal)
        {
            weakSpotHP--;
            bossController.SwitchWeakSpotPos();
            if (weakSpotHP <= 0)
            {
                bossController.SwitchBehavior(BossBehavior.Weakness);
            }
        }
    }

}
