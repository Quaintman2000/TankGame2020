using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AiController : MonoBehaviour
{
    private Vision vision;
    private Hearing hearing;
    [Header("Character Traits")]
    public Material aggressiveColor;
    public Material cowardlyColor;
    public Material sniperColor;
    public Material ambusherColor;
    public enum AIState
    {
        Patrol, Investigate, Advance, Attack, flee, Wait, Aim
    };
    public enum AIPersonality { Aggressive, Cowardly, Sniper, Ambusher };
    public AIState currentAIState;
    public AIState previousAIState;
    public AIPersonality personality;
    [Header("Movement")]
    public Transform[] waypoints;
    public float closeEnough = 1.0f;
    public int currentWaypoint = 0;
    public float defualtInvestigateTimer;
    private float investigateTimer;
    public float fleeTime;
    public float fleeTimer;
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
    public Transform targetTransform;

    public enum LoopType { Stop, Loop, PingPong };
    public LoopType loopType;

    private bool isPatrolFoward = true;

    public float timerDelay = 1.0f;
    private float lastEventTime;

    private Transform visualsObject;
    private Transform body;
    private Transform cannon;
    private GameObject bodyBase;
    private GameObject barrel;
    private GameObject shell;


    // Start is called before the first frame update
    void Start()
    {
        //grab components
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        tf = gameObject.GetComponent<Transform>();
        vision = gameObject.GetComponent<Vision>();
        hearing = gameObject.GetComponent<Hearing>();
        lastEventTime = Time.time;
        //add to the game manager
        GameManager.Instance.enemyDatas.Add(data);
        
        //grab all the visual components
        visualsObject = this.gameObject.transform.GetChild(0);
        body = visualsObject.GetChild(0);
        cannon = visualsObject.GetChild(1);
        shell = body.GetChild(0).gameObject;
        bodyBase = cannon.GetChild(0).gameObject;
        barrel = cannon.GetChild(1).gameObject;

        //sets starting state for each personality
        if(personality == AIPersonality.Sniper || personality == AIPersonality.Ambusher)
        {
            currentAIState = AIState.Patrol;
        }
        else if (personality == AIPersonality.Ambusher || personality == AIPersonality.Cowardly)
        {
            currentAIState = AIState.Wait;
        }

        targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAiPersonalities();

        HandleAiStates();
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
    /// Handles the personalitys of the AI
    /// </summary>
    private void HandleAiPersonalities()
    {
        
        switch(personality)
        {
            case AIPersonality.Aggressive:
                //set the color for the personality
                shell.GetComponent<Renderer>().material = aggressiveColor;
                barrel.GetComponent<Renderer>().material = aggressiveColor;
                bodyBase.GetComponent<Renderer>().material = aggressiveColor;
                //start on wait
                ChangeState(AIState.Wait);
                //check for transitions
                //if ai can see the player
                if(vision.CanSee(targetTransform.gameObject))
                {
                    ChangeState(AIState.Advance);
                }
                else
                {
                    ChangeState(previousAIState);
                }
                break;
            case AIPersonality.Ambusher:
                //set the color for the personality
                shell.GetComponent<Renderer>().material = ambusherColor;
                barrel.GetComponent<Renderer>().material = ambusherColor;
                bodyBase.GetComponent<Renderer>().material = ambusherColor;
                //start on patrol
                ChangeState(AIState.Patrol);
                //check for transitions
                //TODO set ambusher personality
                break;
            case AIPersonality.Cowardly:
                //set the color for the personality
                shell.GetComponent<Renderer>().material = cowardlyColor;
                barrel.GetComponent<Renderer>().material = cowardlyColor;
                bodyBase.GetComponent<Renderer>().material = cowardlyColor;
                //start on wait
                
                //check for transitions
                //if ai can see the player
                if (vision.CanSee(targetTransform.gameObject) == true && currentAIState == AIState.Wait)
                {
                    //start the timer
                    fleeTimer = fleeTime;
                    ChangeState(AIState.flee);
                }
                else if(currentAIState == AIState.flee && fleeTimer > 0)
                {
                    fleeTimer -= Time.deltaTime;
                }
                else if(fleeTimer <= 0 && !(vision.CanSee(targetTransform.gameObject)))
                {
                    ChangeState(AIState.Wait);
                }
               
                break;
            case AIPersonality.Sniper:
                //set the color for the personality
                shell.GetComponent<Renderer>().material = sniperColor;
                barrel.GetComponent<Renderer>().material = sniperColor;
                bodyBase.GetComponent<Renderer>().material = sniperColor;
                
                //if the AI can see the player during patrol or investigate...
                if(vision.CanSee(targetTransform.gameObject) && (currentAIState == AIState.Patrol || currentAIState == AIState.Investigate))
                {
                    //enter aim state
                    ChangeState(AIState.Aim);
                }
                //if the AI can't see the player but can HEAR the player while on patrol...
                else if(hearing.CanHear(targetTransform.gameObject) && currentAIState == AIState.Patrol)
                {
                    //enter investigate state
                    ChangeState(AIState.Investigate);
                    //set the timer
                    investigateTimer = stateEnterTime + defualtInvestigateTimer;
                }
                //if the AI still cant see while in investigate state and the timer runs out...
                else if(currentAIState == AIState.Investigate && !vision.CanSee(targetTransform.gameObject) && Time.time >= investigateTimer)
                {
                    //return to patrol
                    ChangeState(AIState.Patrol);
                }
                //if the AI is aiming and the target is within sights...
                else if(currentAIState == AIState.Aim && vision.WithinSights(targetTransform.gameObject) == true && vision.CanSee(targetTransform.gameObject))
                {
                    //enter attack state
                    ChangeState(AIState.Attack);
                }
                //if the AI is in aim or attack mode AND can't see the target...
                else if((currentAIState == AIState.Aim || currentAIState == AIState.Attack) && !vision.CanSee(targetTransform.gameObject))
                {
                    //enter investigate state
                    ChangeState(AIState.Investigate);
                    //set the timer
                    investigateTimer = stateEnterTime + defualtInvestigateTimer;
                }
                break;
        }
    }
    /// <summary>
    /// Handles all the different Ai States
    /// </summary>
    private void HandleAiStates()
    {
        //handle Ai States
        if (currentAIState == AIState.Patrol)
        {
            if(avoidStage == 1)
            {
                DoAvoidance();
            }
            else
            {
                Patrol();
            }
        }
        else if (currentAIState == AIState.Advance)
        {
            DoAdvance();
        }
        else if (currentAIState == AIState.flee)
        {
            DoRetreat();
        }
        else if (currentAIState == AIState.Wait)
        {
            //do nothing

        }
        else if (currentAIState == AIState.Investigate)
        {
            
            //rotate towards the target it "hears"
            motor.RotateTowards(targetTransform.position, data.rotateSpeed*Time.deltaTime);

        }
        else if (currentAIState == AIState.Attack)
        {
            IntervalShoot();
        }
        else if (currentAIState == AIState.Aim)
        {
            //aim towards target
            motor.RotateTowards(targetTransform.position, data.rotateSpeed*Time.deltaTime);
        }
        else
        {
            Debug.LogError("[AiController Tank: {0}] Invaild AI State", this.gameObject);
        }
    }
    /// <summary>
    /// Does event for flee state (runs away from the player)
    /// </summary>
    private void DoRetreat()
    {
        if (avoidStage != 0)
        {
            DoAvoidance();
        }
        else
        {
            DoFlee();
        }
    }
    /// <summary>
    /// Does event for advance state (runs towards the player)
    /// </summary>
    private void DoAdvance()
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
    /// <summary>
    /// Handles obstancle avoidance
    /// </summary>
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
    /// <summary>
    /// Chases the target
    /// </summary>
    void DoChase()
    {
        motor.RotateTowards(targetTransform.position, data.rotateSpeed);
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
    }
    /// <summary>
    /// flees from target
    /// </summary>
    void DoFlee()
    {
        Vector3 vectorToTarget = targetTransform.position - tf.position;
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
    /// <summary>
    /// shoots at set interval
    /// </summary>
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
    /// <summary>
    /// goes through all waypoints in a set loop
    /// </summary>
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
    /// <summary>
    /// goes back and forth up and down the order of set waypoints
    /// </summary>
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
    /// <summary>
    /// goes to first waypoint and stops at last waypoint set
    /// </summary>
    private void StopLoop()
    {
        if (currentWaypoint < waypoints.Length - 1)
        {
            currentWaypoint++;
        }
    }

   
}
