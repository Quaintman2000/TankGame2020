using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [RequireComponent(typeof(Vision))]
 [RequireComponent(typeof(Hearing))]
public class NeutralFSMController : MonoBehaviour
{
    private Vision vision;
    private Hearing hearing;
    private Transform currentTarget;
    [Header("Character Traits")]
    public Material sniperColor;
    public enum AIState
    {
        Patrol, Charge, Shoot, Investigate
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
        //if there is a player one
        if(GameManager.Instance.playerOneData != null)
        {
            //add it to the transform
            playerOneTransform = GameManager.Instance.playerOneData.GetComponent<Transform>();

            if(vision.CanSee(playerOneTransform.gameObject) && Vector3.Distance(this.transform.position, currentTarget.position) < firingRange)
            {
                playerOneTransform = currentTarget;

                ChangeState(AIState.Shoot);
            }
            else
            {
                currentTarget = null;
                ChangeState(AIState.Patrol);
            }
        }
        else
        {
            playerOneTransform = null;
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
        if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - tf.position) < (closeEnough ))
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
}
