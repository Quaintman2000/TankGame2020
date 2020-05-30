using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletForce = 100.0f;
    public float bulletDuration = 3.0f;
    public float damage = 10;
    public Rigidbody rbody;

    private void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody>();
        rbody.AddForce(transform.forward * bulletForce);
    }
    private void Update()
    {
        //count down the bullet lifetime timer and if its < 0, destroy it
        bulletDuration -= Time.deltaTime;
        if(bulletDuration <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        //if it hits the player
        if (collision.gameObject.tag == "Player")
        {
            //activate its takedamage function
            collision.gameObject.GetComponent<TankMotor>().TakeDamage(damage);
            //destroy the bullet
            Destroy(this.gameObject);
        }
        //if it hits the enemy
        else if (collision.gameObject.tag == "Enemy")
        {
            //activate its takedamage function
            collision.gameObject.GetComponent<TankMotor>().TakeDamage(damage);
            //destroy the bullet
            Destroy(this.gameObject);
        }
        //if it hits anything else
        else
        {
            //destroy the bullet
            Destroy(this.gameObject);
        }
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    //if it hits the player
    //    if (other.gameObject.tag == "Player")
    //    {
    //        //activate its takedamage function
    //        other.gameObject.GetComponent<TankMotor>().TakeDamage(damage);
    //        //destroy the bullet
    //        Destroy(this.gameObject);
    //    }
    //    //if it hits the enemy
    //    else if (other.gameObject.tag == "Enemy")
    //    {
    //        //activate its takedamage function
    //        other.gameObject.GetComponent<TankMotor>().TakeDamage(damage);
    //        //destroy the bullet
    //        Destroy(this.gameObject);
    //    }
    //    //if it hits anything else
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}
}
