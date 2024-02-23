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
}
