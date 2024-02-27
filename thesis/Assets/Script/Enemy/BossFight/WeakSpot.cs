using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    public BossController bossController;
    public int weakSpotHP;

    private void Start()
    {
        ResetWeakSpotHP();
    }

    public void ResetWeakSpotHP()
    {
        weakSpotHP = bossController.bossSO.weakSpotHP;
    }

    public void RemoveWeakSpotHP()
    {
        weakSpotHP--;
        if (weakSpotHP <= 0)
        {
            bossController.SwitchBehavior(BossBehavior.Weakness);
        }
    }

}
