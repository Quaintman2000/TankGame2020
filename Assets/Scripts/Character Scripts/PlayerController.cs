using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private TankData data;
    private TankMotor motor;
    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<TankData>();
        motor = GetComponent<TankMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            motor.Move(data.moveSpeed);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            motor.Rotate(data.rotateSpeed);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            motor.Rotate(-data.rotateSpeed);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            motor.Move(-data.moveSpeed);
        }
    }
}
