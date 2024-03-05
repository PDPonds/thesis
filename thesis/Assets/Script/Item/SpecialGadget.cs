using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GadgetType
{
    Projectile, UseForPlayer
}

public class SpecialGadget : ScriptableObject
{
    public Sprite gadgetSprint;
    public int maxStack;
    public GadgetType gadgetType;

    public virtual void UseGadget() { }

}
