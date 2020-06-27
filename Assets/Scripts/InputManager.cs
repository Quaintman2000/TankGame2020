using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    private TankData data;
    private TankMotor motor;
    public enum InputScheme { WASD, arrowKeys};
    public InputScheme input = InputScheme.WASD;

    public float timerdelay = 1.0f;
    private float coolDownTime;
     bool canShoot
    {
        get
        {
            return (coolDownTime <= 0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
        GameManager.Instance.playerData = data;
    }

    // Update is called once per frame
    void Update()
    {
        //checks for inputs
        HandleInput();
        //if cant shoot, count down the timer
        if(!canShoot)
        {
            coolDownTime -= Time.deltaTime;
        }
    }
    void HandleInput()
    {
        //checks for input schemes
        switch (input)
        {
            //if the scheme are the arrow keys
            case InputScheme.arrowKeys:
                //if pressed the up arrow
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    //move forward
                    motor.Move(data.moveSpeed);
                }
                //if pressed the down arrow
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    //move backward
                    motor.Move(data.reverseSpeed);
                }
                //if pressed neither the up or down arrow
                else
                {
                    //dont move
                    motor.Move(0);
                }
                //if pressed the right arrow
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    //turn right
                    motor.Rotate(data.rotateSpeed);
                }
                //if pressed the left arrow
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    //turn left
                    motor.Rotate(-data.rotateSpeed);
                }
                //if pressed the space bar
                if (Input.GetKey(KeyCode.Space))
                {
                    //reset the timer
                    if (canShoot)
                    {
                        //shoot the bullet
                        motor.Shoot();
                        //reset the timer
                        coolDownTime = timerdelay;
                    }
                }
                break;
            //if the scheme are the WASD keys
            case InputScheme.WASD:
                if (Input.GetKey(KeyCode.W))
                {
                    //move forward
                    motor.Move(data.moveSpeed);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    //move backward
                    motor.Move(data.reverseSpeed);
                }
                else
                {
                    //dont move
                    motor.Move(0);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    //turn right
                    motor.Rotate(data.rotateSpeed);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    //turn left
                    motor.Rotate(-data.rotateSpeed);
                }
                if (Input.GetKey(KeyCode.Space))
                {
                    //reset the timer
                    if (canShoot)
                    {
                        //shoot the bullet
                        motor.Shoot();
                        //reset the timer
                        coolDownTime = timerdelay;
                        //send message to make"noise"
                        gameObject.SendMessage("AddNoise", 10,SendMessageOptions.DontRequireReceiver);
                    }
                }
                break;
            //if the scheme is invalid
            default:
                Debug.LogError("[InputManager] undefined input scheme.");
                break;
        }
    }
}
