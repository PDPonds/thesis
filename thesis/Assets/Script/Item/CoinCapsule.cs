using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCapsule : MonoBehaviour
{
    public int dropCointMin;
    public int dropCoinMax;
    [SerializeField] GameObject attackInfo;

    private void Update()
    {
        float dis = Vector2.Distance(transform.position, PlayerManager.Instance.transform.position);
        if (dis < 8f)
            attackInfo.SetActive(true);
        else
            attackInfo.SetActive(false);
    }


}
