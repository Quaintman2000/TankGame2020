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

    private Transform visualsObject;
    private Transform body;
    private Transform cannon;
    private GameObject bodyBase;
    private GameObject barrel;
    private GameObject shell;

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
        
    }
}
