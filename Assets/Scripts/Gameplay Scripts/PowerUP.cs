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
    public int scoreMod;

    public float duration;
    public bool isPernament;

    public void OnActivate(TankData target)
    {
        target.moveSpeed += speedMod;
        target.health += healthMod;
        target.maxHealth += maxHealthMod;
        target.fireRate += fireRateMod;
        if(target.gameObject.name == "PlayerTank")
        {
            GameManager.Instance.playerOneScore += scoreMod;
        }
        else if(target.gameObject.name == "Player2Tank")
        {
            GameManager.Instance.playerTwoScore += scoreMod;
        }
    }
    public void OnDeactivate(TankData target)
    {
        target.moveSpeed -= speedMod;
        target.health -= healthMod;
        target.maxHealth -= maxHealthMod;
        target.fireRate -= fireRateMod;
    }
}
