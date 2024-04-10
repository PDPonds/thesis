using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public bool passTutorial;
    public int coin;

    public int maxHPLevel;
    public int hpLeechLevel;
    public int reviveCount;

    public PlayerData()
    {
        coin = PlayerManager.coin;
        maxHPLevel = PlayerManager.upgradeMaxHpLevel;
        hpLeechLevel = PlayerManager.upgradeStealHpLevel;
        reviveCount = PlayerManager.reviveItemCount;
        passTutorial = PlayerManager.passTutorial;
    }

}
