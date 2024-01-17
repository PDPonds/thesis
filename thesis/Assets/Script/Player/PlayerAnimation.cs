using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerManager.Instance.onAttack += PlayAttackAnimation;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.onAttack -= PlayAttackAnimation;
    }

    private void Update()
    {
        Animator anim = PlayerManager.Instance.anim;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                PlayerManager.Instance.attackCol.enabled = false;
            }
        }
        
    }

    void PlayAttackAnimation()
    {
        Animator anim = PlayerManager.Instance.anim;
        anim.Play("Attack");
    }

}
