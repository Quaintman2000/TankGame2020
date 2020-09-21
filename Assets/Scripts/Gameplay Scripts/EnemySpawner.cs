using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] waypoints;
    private GameObject spawnedEntity;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.enemySpawns.Add(this); 
    }

    public void SpawnEnemy(GameObject enemy)
    {
       //spawn the enemy
        spawnedEntity=Instantiate(enemy, this.gameObject.transform.position, this.gameObject.transform.rotation);

        //if the enemy is the sniper or aggressive
        if (spawnedEntity.GetComponent<SniperFSMController>() != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                spawnedEntity.GetComponent<SniperFSMController>().waypoints.Add(waypoints[i].transform);
            }
        }
        else if(spawnedEntity.GetComponent<CowardlyFSMController>() != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                spawnedEntity.GetComponent<CowardlyFSMController>().waypoints.Add(waypoints[i].transform);
            }
        }
        else if(spawnedEntity.GetComponent<NeutralFSMController>() != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                spawnedEntity.GetComponent<NeutralFSMController>().waypoints.Add(waypoints[i].transform);
            }
        }
        else
        {
            Debug.LogError(this.gameObject.name + " Cannot input waypoints into spawned enity.");
        }
    }
}
