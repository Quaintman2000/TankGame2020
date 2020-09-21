using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMotor))]
[RequireComponent(typeof(TankData))]
public class AiController2 : MonoBehaviour
{
    public Transform target;
    public enum AttackMode { Chase, Flee};
    public AttackMode attackMode;
    
    private TankData data;
    private TankMotor motor;
    private Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<TankData>();
        motor = GetComponent<TankMotor>();
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attackMode == AttackMode.Chase)
        {
            //rotate towards the targert
            motor.RotateTowards(target.position, data.rotateSpeed * Time.deltaTime);
            //move towards player
            motor.Move(data.moveSpeed);
        }
        if(attackMode == AttackMode.Flee)
        {
            Vector3 vectorToTarget = target.position - tf.position;
            Vector3 awayFromTarget = -vectorToTarget;
            
            awayFromTarget.Normalize();
            Vector3 fleePosition = awayFromTarget + tf.position;
            motor.RotateTowards(fleePosition, data.rotateSpeed );
            motor.Move(data.moveSpeed);
        }
    }
}
