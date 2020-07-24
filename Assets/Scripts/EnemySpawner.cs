using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.enemySpawns.Add(this); 
    }

    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, this.gameObject.transform.position, this.gameObject.transform.rotation);
    }
}
