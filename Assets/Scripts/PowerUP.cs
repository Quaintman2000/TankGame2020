using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUP
{
    public float speedMod;
    public float healthMod;
    public float maxHealthMod;
    public float fireRateMod;

    public float duration;
    public bool isPernament;

    public void OnActivate(TankData target)
    {
        target.moveSpeed += speedMod;
        target.health += healthMod;
        target.maxHealth += maxHealthMod;
        target.fireRate += fireRateMod;
    }
    public void OnDeactivate(TankData target)
    {
        target.moveSpeed -= speedMod;
        target.health -= healthMod;
        target.maxHealth -= maxHealthMod;
        target.fireRate -= fireRateMod;
    }
}
