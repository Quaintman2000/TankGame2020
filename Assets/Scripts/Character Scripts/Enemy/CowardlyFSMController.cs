﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Vision))]
[RequireComponent(typeof(Hearing))]
public class CowardlyFSMController : MonoBehaviour
{
    private Vision vision;
    private Hearing hearing;
    private Transform currentTarget;
    [Header("Character Traits")]
    public Material sniperColor;
    public enum AIState
    {
        Patrol, Charge, Shoot, Investigate, flee
    };
    public AIState currentAIState;
    public AIState previousAIState;
    [Header("Movement")]
    public List<Transform> waypoints = new List<Transform>();
    public float closeEnough = 1.0f;
    public int currentWaypoint = 0;

    public float defualtInvestigateTimer;
    private float investigateTimer;
    public float stateEnterTime;

    public bool CanMove(float speed)
    {
        //check if the ai can move forward in "speed" distance 
        //send a raycast to see if we cant move
        RaycastHit hit;
        if (Physics.Raycast(tf.position, tf.forward, out hit, speed))
        {
            //and if it is anything BUT the player
            if (!hit.collider.CompareTag("Player"))
            {
                return false;
            }
        }
        return true;
    }
    public int avoidStage = 0;
    public float avoidTime = 2.0f;
    private float exitTime;

    private TankData data;
    private TankMotor motor;
    private Transform tf;
    public Transform playerOneTransform;
    public Transform playerTwoTransform;

    public enum LoopType { Stop, Loop, PingPong };
    public LoopType loopType;

    private bool isPatrolFoward = true;

    public float timerDelay = 1.0f;
    private float lastEventTime;

    public float firingRange;
    // Start is called before the first frame update
    void Start()
    {
        //get necessary components
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        tf = gameObject.GetComponent<Transform>();
        vision = gameObject.GetComponent<Vision>();
        hearing = gameObject.GetComponent<Hearing>();
        lastEventTime = Time.time;
        //add to the game manager
        GameManager.Instance.enemyDatas.Add(data);

        //start on patrol state
        currentAIState = AIState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        //cowardly FSM
        //if there is a player 
        if (GameManager.Instance.playerOneData != null || GameManager.Instance.playerTwoData != null)
        {
            if (GameManager.Instance.playerOneData != null)
            {
                playerOneTransform = GameManager.Instance.playerOneData.transform;
            }
            if (GameManager.Instance.playerTwoData != null)
            {
                playerTwoTransform = GameManager.Instance.playerTwoData.transform;
            }
            

            //check to see if you can see the player
            if (vision.CanSee(playerOneTransform.gameObject))
            {
                currentTarget = playerOneTransform;

                //check if the player is facing the AI
                if (currentTarget.gameObject.GetComponent<Vision>().CanSee(this.gameObject))
                {
                    //flee
                    ChangeState(AIState.flee);
                }
                else
                {
                    //charge
                    currentAIState = AIState.Charge;

                    if (Vector3.Distance(this.transform.position, currentTarget.position) < firingRange)
                    {
                        ChangeState(AIState.Shoot);
                    }
                }
            }
            else if (hearing.CanHear(playerOneTransform.gameObject) )
            {
                currentTarget = playerOneTransform;
                //enter investigate state;
                currentAIState = AIState.Investigate;
            }
            else if (GameManager.Instance.playerTwoData != null)
            {
                //check to see if you can see the player
                if (vision.CanSee(playerTwoTransform.gameObject))
                {
                    currentTarget = playerTwoTransform;

                    //check if the player is facing the AI
                    if (currentTarget.gameObject.GetComponent<Vision>().CanSee(this.gameObject) )
                    {
                        //flee
                        ChangeState(AIState.flee);
                    }
                    else
                    {
                        //charge
                        currentAIState = AIState.Charge;

                        if (Vector3.Distance(this.transform.position, currentTarget.position) < firingRange)
                        {
                            ChangeState(AIState.Shoot);
                        }
                    }
                }
                else if (hearing.CanHear(playerTwoTransform.gameObject))
                {
                    currentTarget = playerOneTransform;
                    //enter investigate state;
                    currentAIState = AIState.Investigate;
                }
                //if not
                else
                {
                    //clear target
                    currentTarget = null;
                    //continue patroling
                    ChangeState(AIState.Patrol);
                }
            }
        }
        //if not
        else
        {
            //clear target
            currentTarget = null;
            //continue patroling
            ChangeState(AIState.Patrol);
        }
      
        AIStateHandler();
    }
    private void AIStateHandler()
    {
        //if the current ai state is patroling
        switch (currentAIState)
        {
            case AIState.Patrol:
                //patrol
                Patrol();
                break;
            case AIState.Charge:
                //rotate towards the target
                motor.RotateTowards(currentTarget.position, data.rotateSpeed * Time.deltaTime);

                //check to see if we can move in that direction
                if (CanMove(data.moveSpeed))
                {
                    //move in that direction
                    motor.Move(data.moveSpeed);
                }
                else
                {
                    //start avoiding
                    avoidStage = 1;
                }
                break;
            case AIState.Investigate:
                //rotate towards the target it "hears"
                motor.RotateTowards(currentTarget.position, data.rotateSpeed * Time.deltaTime);
                break;
            case AIState.Shoot:
                //keep aiming at the target player
                motor.RotateTowards(currentTarget.position, data.rotateSpeed * Time.deltaTime);

                //and shoot
                IntervalShoot();
                break;
            case AIState.flee:
                // do flee stuff
                if (currentTarget.gameObject.GetComponent<Vision>().CanSee(this.gameObject))
                {
                    DoRetreat();
                }
                else
                {
                    ChangeState(AIState.Charge);
                }
                break;

        }
    }

    public void ChangeState(AIState newState)
    {
        //save the previous state.
        previousAIState = currentAIState;

        //change to new state.
        currentAIState = newState;

        //save the time we changed states at.
        stateEnterTime = Time.time;
    }
    /// <summary>
    /// Handles the patrol
    /// </summary>
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
        if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - tf.position) < (closeEnough))
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
    /// <summary>
    /// goes through all waypoints in a set loop
    /// </summary>
    private void LoopLoop()
    {
        if (currentWaypoint < waypoints.Count - 1)
        {
            currentWaypoint++;
        }
        else
        {
            currentWaypoint = 0;
        }
    }
    /// <summary>
    /// goes back and forth up and down the order of set waypoints
    /// </summary>
    private void PingPongLoop()
    {
        if (isPatrolFoward)
        {
            if (currentWaypoint < waypoints.Count - 1)
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
    /// <summary>
    /// goes to first waypoint and stops at last waypoint set
    /// </summary>
    private void StopLoop()
    {
        if (currentWaypoint < waypoints.Count - 1)
        {
            currentWaypoint++;
        }
    }
    /// <summary>
    /// shoots at set interval
    /// </summary>
    private void IntervalShoot()
    {
        //Shoot at an interval
        if (Time.time >= lastEventTime + timerDelay)
        {
            motor.Shoot();
            lastEventTime = Time.time;
        }
    }
    void DoAvoidance()
    {
        if (avoidStage == 1)
        {
            //turn left
            motor.Rotate(-data.rotateSpeed);

            //if we can move forward
            if (CanMove(data.moveSpeed))
            {
                avoidStage = 2;

                //set the avoidance timer
                exitTime = avoidTime;
            }
        }
        else if (avoidStage == 2)
        {
            //check if we can move forward then keep moving
            if (CanMove(data.moveSpeed))
            {
                //count down the avoid time
                exitTime -= Time.deltaTime;
                motor.Move(data.moveSpeed);

                //if we ran out of time, try chasing again
                if (exitTime <= 0)
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
    private void DoRetreat()
    {
        if (avoidStage != 0)
        {
            DoAvoidance();
        }
        else
        {
            DoFlee(currentTarget);
        }
    }
    /// <summary>
    /// flees from target
    /// </summary>
    void DoFlee(Transform target)
    {
        Vector3 vectorToTarget = target.position - tf.position;
        Vector3 awayFromTarget = -vectorToTarget;

        awayFromTarget.Normalize();
        Vector3 fleePosition = awayFromTarget + tf.position;
        motor.RotateTowards(fleePosition, data.rotateSpeed);
        if (CanMove(data.moveSpeed))
        {
            motor.Move(data.moveSpeed);
        }
        else
        {
            avoidStage = 1;
        }
    }
}
