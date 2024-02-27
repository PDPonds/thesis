using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss")]
public class BossSO : ScriptableObject
{
    public int maxHp;
    public int dropCoin;
    public int dropScore;
}
