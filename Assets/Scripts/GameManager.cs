using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : Singleton<GameManager>
{

    private const int NEWHIGHSCORECOUNT = 3;
    public GameManager instance;
    public GameObject playerPrefab;
    public GameObject[] enemyTankPrefabs;
    public TankData playerData;
    public List<TankData> enemyDatas = new List<TankData>();
    public List<PlayerSpawner> playerSpawns = new List<PlayerSpawner>();
    public List<EnemySpawner> enemySpawns = new List<EnemySpawner>();
    public List<Pickup> powerUps = new List<Pickup>();
    public List<ScoreData> scoreDatas = new List<ScoreData>();

    protected override void Awake()
    {
        base.Awake();
        scoreDatas.Sort();
        scoreDatas.Reverse();
        scoreDatas = scoreDatas.GetRange(index: 0, count: NEWHIGHSCORECOUNT);
    }
    private void Update()
    {
      

        if(playerData == null)
        {
            //random spawn
            int randomSpawn = UnityEngine.Random.Range(0, enemySpawns.Count - 1);
            //spawn tank
            playerSpawns[randomSpawn].SpawnPlayer(playerPrefab);
        
        }

        //check the number of enemies in the world
        //if there are less than 4 enemies in the world
        if (enemyDatas.Count < 4)
        {
            //random spawn
            int randomSpawn = UnityEngine.Random.Range(0, enemySpawns.Count - 1);
            //random tank
            int randomTank = UnityEngine.Random.Range(0, enemyTankPrefabs.Length - 1);
            //spawn enemy
            enemySpawns[randomSpawn].SpawnEnemy(enemyTankPrefabs[randomTank]);

        }
    }
}
