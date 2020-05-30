using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameManager instance;
    public GameObject playerPrefab;
    public TankData playerData;
    public List<TankData> enemyDatas = new List<TankData>();


}
