using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;
    public List<GameObject> pickupPrefabs;
    public GameObject currentPickup;
    public float spawnDelay;
    private float nextSpawnTime;
    private Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
        nextSpawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentPickup == null)
        {
            if (Time.time > nextSpawnTime)
            {
                pickupPrefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Count)];
                currentPickup = Instantiate(pickupPrefab, tf.position, tf.rotation);
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else
        {
            nextSpawnTime = Time.time + spawnDelay;
        }
       
    }
}
