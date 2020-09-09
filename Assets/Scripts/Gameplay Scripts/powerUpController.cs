using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
public class powerUpController : MonoBehaviour
{
    // Start is called before the first frame update
    private TankData data;
    public List<PowerUP> powerUps;
    void Start()
    {
        data = GetComponent<TankData>();
        powerUps = new List<PowerUP>();
    }

    // Update is called once per frame
    void Update()
    {
        List<PowerUP> expiredPowerups = new List<PowerUP>();
        
        foreach(PowerUP power in powerUps)
        {
            power.duration -= Time.deltaTime;

            if(power.duration <= 0)
            {
                expiredPowerups.Add(power);
            }
        }

        foreach(PowerUP expiredPower in expiredPowerups)
        {
            expiredPower.OnDeactivate(data);
            powerUps.Remove(expiredPower);
        }
        expiredPowerups.Clear();
    }

    public void AddPowerup(PowerUP power)
    {
        power.OnActivate(data);//activates the power up
        //only keep track of temp powerups
        if(!power.isPernament)
        {
            powerUps.Add(power);
        }
    }
}
