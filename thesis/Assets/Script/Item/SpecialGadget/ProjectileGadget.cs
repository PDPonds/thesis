using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpecialGadget/ProjectileGadget")]
public class ProjectileGadget : SpecialGadget
{
    public float bulletSpeed;
    public GameObject projectilePrefab;

    public ProjectileGadget()
    {
        gadgetType = GadgetType.Projectile;
    }

    public override void UseGadget()
    {
        if (PlayerManager.Instance.gadgetSlot.gadget != null)
        {
            if (PlayerManager.Instance.gadgetSlot.gadget == this)
            {
                GameObject projectileObj = Instantiate(projectilePrefab,
                    PlayerManager.Instance.shurikenSpawnPoint.position, Quaternion.identity);

                Rigidbody2D rb = projectileObj.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.right * bulletSpeed, ForceMode2D.Impulse);

                PlayerManager.Instance.RemoveGadget(1);

                SoundManager.Instance.PlayOnShot("ShurikenThrow");

            }
        }
    }
}
