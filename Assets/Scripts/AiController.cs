using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    private TankData data;
    private TankMotor motor;
    public float timerDelay = 1.0f;
    private float lastEventTime;
    // Start is called before the first frame update
    void Start()
    {
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        lastEventTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot at an interval
        if (Time.time >= lastEventTime + timerDelay)
        {
            motor.Shoot();
            Debug.Log("It's me!");
            lastEventTime = Time.time;
        }
    }
}
