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
       
        spawnedEntity=Instantiate(enemy, this.gameObject.transform.position, this.gameObject.transform.rotation);
        for(int i =0; i<waypoints.Length; i++)
        {
            spawnedEntity.GetComponent<AiController>().waypoints.Add(waypoints[i].transform);
        }
    }
}
