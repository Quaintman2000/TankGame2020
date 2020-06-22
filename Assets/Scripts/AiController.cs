using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : MonoBehaviour
{
    public enum AIState
    {
        Patrol, WaitForBackup, Advance, Attack, flee
    };
    public enum AIPersonality { Aggressive, Cautious };
    public AIState currentAIState;
    public AIPersonality personality;
    public Transform[] waypoints;
    public float closeEnough = 1.0f;
    public int currentWaypoint = 0;

    public bool CanMove(float speed)
    {
        //check if the ai can move forward in "speed" distance 
        //send a raycast to see if we cant move
        RaycastHit hit;
        if(Physics.Raycast (tf.position, tf.forward, out hit, speed))
        {
            //and if it is anything BUT the player
            if(!hit.collider.CompareTag("Player"))
            {
                return false;
            }
        }
        return true;
    }
    private int avoidStage = 0;
    public float avoidTime = 2.0f;
    private float exitTime;

    private TankData data;
    private TankMotor motor;
    private Transform tf;
    public Transform targetTransform;

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
        //handle Ai States
        if (currentAIState == AIState.Patrol)
        {
            Patrol();
        }
        else if (currentAIState == AIState.Advance)
        {
            if (avoidStage != 0)
            {
                DoAvoidance();
            }
            else
            {
                DoChase();
            }
        }
    }

    void DoAvoidance()
    {
        if(avoidStage == 1)
        {
            //turn left
            motor.Rotate(-data.rotateSpeed);

            //if we can move forward
            if(CanMove(data.moveSpeed))
            {
                avoidStage = 2;

                //set the avoidance timer
                exitTime = avoidTime;
            }
        }
        else if(avoidStage ==2)
        {
            //check if we can move forward then keep moving
            if(CanMove(data.moveSpeed))
            {
                //count down the avoid time
                exitTime -= Time.deltaTime;
                motor.Move(data.moveSpeed);

                //if we ran out of time, try chasing again
                if(exitTime <= 0)
                {
                    avoidStage = 0;
                }
            }
            else
            {
                //if we cant, go back to stage one.
                avoidStage = 1;
            }
        }
    }
    void DoChase()
    {
        motor.RotateTowards(targetTransform.position, data.rotateSpeed);
        //check to see if we can move in that direction
        if(CanMove(data.moveSpeed))
        {
            //move in that direction
            motor.Move(data.moveSpeed);
        }
        else
        {
            //start avoiding
            avoidStage = 1;
        }
    }
    private void IntervalShoot()
    {
        //Shoot at an interval
        if (Time.time >= lastEventTime + timerDelay)
        {
            motor.Shoot();
            Debug.Log("It's me!");
            lastEventTime = Time.time;
        }
    }

    private void Patrol()
    {
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
            else if (loopType == LoopType.Loop)
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
