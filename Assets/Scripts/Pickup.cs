using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PowerUP powerup;
    public AudioClip feedbackAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        //add this to the game manager
        GameManager.Instance.powerUps.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        //get the other object's powerup controller
        powerUpController powerUpController = other.gameObject.GetComponent<powerUpController>();

        if (powerUpController != null)
        {
            powerUpController.AddPowerup(powerup);

            if(feedbackAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(feedbackAudioClip, transform.position, 1.0f);
            }

            Destroy(this.gameObject);
        }
    }
    //when picked up
    private void OnDestroy()
    {
        //remove from the game manager
        GameManager.Instance.powerUps.Remove(this);
    }
}
