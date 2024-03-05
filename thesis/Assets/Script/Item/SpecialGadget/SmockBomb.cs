using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpecialGadget/SmockBomb")]
public class SmockBomb : SpecialGadget
{
    public float noDamageTime;

    public SmockBomb()
    {
        gadgetType = GadgetType.UseForPlayer;
    }

    public override void UseGadget()
    {
        if (PlayerManager.Instance.gadgetSlot.gadget != null)
        {
            if (PlayerManager.Instance.gadgetSlot.gadget == this)
            {
                Vector3 playerPos = PlayerManager.Instance.transform.position;
                GameObject smoke = GameManager.Instance.smokeParticle.gameObject;
                GameManager.Instance.SpawnParticle(smoke, playerPos);
                PlayerManager.Instance.curNoDamageTime = noDamageTime;
                PlayerManager.Instance.noDamage = true;

                Physics2D.IgnoreLayerCollision(3, 7, true);

                PlayerManager.Instance.RemoveGadget(1);

            }
        }
    }

}
