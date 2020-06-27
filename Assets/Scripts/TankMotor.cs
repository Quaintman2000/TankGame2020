using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(TankData))]
public class TankMotor : MonoBehaviour
{
    //need a reference to the character controller component
    private CharacterController characterController;
    private TankData data;
    public GameObject firePoint;
    public GameObject bulletPrefab;
    public AudioClip shootSound;
    private void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();
        data = gameObject.GetComponent<TankData>();
        data.health = data.maxHealth;
    }
    private void Update()
    {
        if(data.health <= 0)
        {
            Die();
        }
    }
    //handle moving the tank
    public void Move(float speed)
    {
        // create a vector to move the tank forward
        Vector3 speedVector = transform.forward * speed;

        //send speedVector to simplemove to handle movement
        characterController.SimpleMove(speedVector);
    }
    //handle rotating the tank
    public void Rotate(float speed)
    {
        //create a vector to hold our rotation data
        //start by rotating by one degree pre fram draw
        //adjust rotation based off speed
        //multiply by time to ensure framerate independence
        Vector3 rotateVector = Vector3.up * speed * Time.deltaTime; ;

        //pass our rotation vector into transform.rotate
        transform.Rotate(rotateVector, Space.Self);
    }
    /// <summary>
    /// Rotate toward a target
    /// </summary>
    /// <param name="target">target to rotate towards</param>
    /// <param name="speed">rotation speed</param>
    /// <returns>Returns true if rotated to the target, false if not.</returns>
    public bool RotateTowards(Vector3 target, float speed)
    {
        //todo: write this function
        //find target
        Vector3 vectorToTarget = target - this.transform.position;
        //find the rotation needed
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget);

        if (targetRotation == this.transform.rotation)
        {
            return false;
        }

        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, targetRotation, speed);
        return true;
    }
    public void Shoot()
    {
        //Spawns the bullet at the firepoint position and rotation and plays the shoot sound
        Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        AudioSource.PlayClipAtPoint(shootSound, firePoint.transform.position);
        gameObject.SendMessage("AddNoise", 5, SendMessageOptions.DontRequireReceiver);
    }

    public void TakeDamage(float damage)
    {
        //reduce the health based on bullet damage
        data.health -= damage;
    }
    public void Die()
    {
        Destroy(this.gameObject);
    }
}
