using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public Transform[] waypoints;
    public float closeEnough = 1.0f;
    public int currentWaypoint = 0;

    private TankData data;
    private TankMotor motor;
    private Transform tf;

    public enum LoopType { Stop, Loop, PingPong };
    public LoopType loopType;

    private bool isPatrolFoward = true;

    public float timerDelay = 1.0f;
    private float lastEventTime;
    // Start is called before the first frame update
    void Start()
    {
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        tf = gameObject.GetComponent<Transform>();
        lastEventTime = Time.time;
        GameManager.Instance.enemyDatas.Add(data);
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
        //rotate towards waypoint
        if (motor.RotateTowards(waypoints[currentWaypoint].position, data.rotateSpeed * Time.deltaTime))
        {
            // do nothing
        }
        else
        {
            motor.Move(data.moveSpeed);
        }
        if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - tf.position) < (closeEnough * closeEnough))
        {
            if (loopType == LoopType.Stop)
            {
                StopLoop();
            }
            else if (loopType == LoopType.PingPong)
            {
                PingPongLoop();
            }
            else if(loopType == LoopType.Loop)
            {
                LoopLoop();
            }
            else
            {
                Debug.LogError("[AiController] unimplemented loop type");
            }

        }
    }

    private void LoopLoop()
    {
        if (currentWaypoint < waypoints.Length - 1)
        {
            currentWaypoint++;
        }
        else
        {
            currentWaypoint = 0;
        }
    }

    private void PingPongLoop()
    {
        if (isPatrolFoward)
        {
            if (currentWaypoint < waypoints.Length - 1)
            {
                currentWaypoint++;
            }
            else
            {
                isPatrolFoward = false;
                currentWaypoint--;
            }

        }
        else
        {
            if (currentWaypoint > 0)
            {
                currentWaypoint--;
            }
            else
            {
                isPatrolFoward = true;
                currentWaypoint++;
            }
        }
    }

    private void StopLoop()
    {
        if (currentWaypoint < waypoints.Length - 1)
        {
            currentWaypoint++;
        }
    }
}
