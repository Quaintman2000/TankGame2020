using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    List<GameObject> waypoints = new List<GameObject>();
    private GameObject spawnedEntity;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.enemySpawns.Add(this); 
    }

    public void SpawnEnemy(GameObject enemy)
    {
        count = 0;
        spawnedEntity=Instantiate(enemy, this.gameObject.transform.position, this.gameObject.transform.rotation);
        for(int i =0; i<waypoints.Count; i++)
        {
            spawnedEntity.GetComponent<AiController>().waypoints[i] = waypoints[i].transform;
        }
    }
}
