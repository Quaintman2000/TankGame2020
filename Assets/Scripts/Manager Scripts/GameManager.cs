using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : Singleton<GameManager>
{
    //Game Manager Stuff
    [Header("Game Manager stuff:")]
    public GameManager instance;
    public GameObject mapGeneratorPrefab;
    public GameObject mapGenerator;
    //gameplay state stuff
    public enum gameplayState{ startMenu, gameRunning, optionsMenu, pauseMenu, endScreen,enterName};
    public gameplayState gameState = gameplayState.startMenu;
    //player stuff
    [Header("Player(s):")]
    public GameObject playerPrefab;
    public GameObject player2Prefab;
    public TankData playerOneData;
    public TankData playerTwoData;
    public List<PlayerSpawner> playerSpawns = new List<PlayerSpawner>();
    public int playerStartingLives;
    public int playerOneLives;
    public int playerTwoLives;
    public int playerOneScore;
    public int playerTwoScore;
    public string playerOneName;
    public string PlayerTwoName;
    //enemy stuff
    [Header("Enemys:")]
    public GameObject[] enemyTankPrefabs;
    public List<TankData> enemyDatas = new List<TankData>();
    public List<EnemySpawner> enemySpawns = new List<EnemySpawner>();
    //power up stuff
    [Header("Power Ups:")]
    public List<Pickup> powerUps = new List<Pickup>();
    public List<PickupSpawner> pickupSpawners = new List<PickupSpawner>();
    // title screen stuff
    [Header("UI Stuff:")]
    public GameObject startScreenComponents;
    public GameObject optionsMenuComponents;
    public GameObject endScreenComponents;
    public GameObject EnterNameScreenComponents;
    //option menu stuff
    [Header("Options Stuff")]
    public bool mulitplayer = false;
    public float musicVolume = 1;
    public float fxVolume = 1;
    public bool MapOfDayToggle = false;
    //High score stuff
    [Header("High Score Stuff:")]
    public List<ScoreData> scoreDatas = new List<ScoreData>();


    protected override void Awake()
    {
        base.Awake();
    }


    private void Update()
    {
        //hand the the UI menu
        UIMenuHandler();

        //handle spawning player and enemies
        //spawn only when the game is in game running state
        if (gameState == gameplayState.gameRunning && mapGenerator != null)
        {


            
            PlayerSpawnHandler();
            EnemySpawnHandler();
            //handle gameover
            //if both player's lives = 0
            if (playerTwoLives == 0 && playerOneLives == 0 )
            {
                //set gamestate to gameover
                gameState = gameplayState.endScreen;

                //clear the unneccesary
                Destroy(mapGenerator);
                foreach (TankData enemy in enemyDatas)
                {
                    Destroy(enemy.gameObject);
                }
                for (int i = 0; i < playerSpawns.Count; i++)
                {
                    Destroy(playerSpawns[i]);
                    Destroy(enemySpawns[i]);
                }
                foreach(PickupSpawner pickup in pickupSpawners)
                {
                    Destroy(pickup.gameObject);
                }
                foreach(Pickup pickup1 in powerUps)
                {
                    Destroy(pickup1.gameObject);
                }
                playerSpawns.Clear();
                enemySpawns.Clear();
                pickupSpawners.Clear();
                powerUps.Clear();
                enemyDatas.Clear();
            }
        }
      
    }

    private void EnemySpawnHandler()
    {
        //check the number of enemies in the world
        //if there are less than 4 enemies in the world
        if (enemyDatas.Count < 4 && enemySpawns.Count >= 1)
        {
            //random spawn
            int randomSpawn = UnityEngine.Random.Range(0, enemySpawns.Count - 1);
            //random tank
            int randomTank = UnityEngine.Random.Range(0, enemyTankPrefabs.Length - 1);
            //spawn enemy
            enemySpawns[randomSpawn].SpawnEnemy(enemyTankPrefabs[randomTank]);

        }
    }

    private void PlayerSpawnHandler()
    {
        //if there is spawns to be spawned in
        if (playerSpawns.Count >= 1)
        {
            //if theres no player one
            if (playerOneData == null && playerOneLives > 0)
            {
                //random spawn
                int randomSpawn = UnityEngine.Random.Range(0, playerSpawns.Count - 1);
                //spawn tank
                playerSpawns[randomSpawn].SpawnPlayer(playerPrefab);
            }
            //if multiplayer is enabled and theres no player two
            if (playerTwoData == null && mulitplayer == true && playerTwoLives > 0)
            {
                //random spawn
                int randomSpawn = UnityEngine.Random.Range(0, enemySpawns.Count - 1);
                //spawn tank
                playerSpawns[randomSpawn].SpawnPlayer(player2Prefab);
            }
        }
    }

    /// <summary>
    /// Handles game states
    /// </summary>
    private void UIMenuHandler()
    {
        switch (gameState)
        {
            case gameplayState.startMenu:
                //enable start menu components only
                startScreenComponents.SetActive(true);
                optionsMenuComponents.SetActive(false);
                endScreenComponents.SetActive(false);
                EnterNameScreenComponents.SetActive(false);
                break;
            case gameplayState.pauseMenu:
                // set up pause menu components here.
                break;
            case gameplayState.gameRunning:
                //disable all menu components
                startScreenComponents.SetActive(false);
                optionsMenuComponents.SetActive(false);
                endScreenComponents.SetActive(false);
                EnterNameScreenComponents.SetActive(false);
                break;
            case gameplayState.endScreen:
                //enable end menu components only
                startScreenComponents.SetActive(false);
                optionsMenuComponents.SetActive(false);
                endScreenComponents.SetActive(true);
                EnterNameScreenComponents.SetActive(false);
                break;
            case gameplayState.optionsMenu:
                //enable options menu components only
                startScreenComponents.SetActive(false);
                optionsMenuComponents.SetActive(true);
                endScreenComponents.SetActive(false);
                EnterNameScreenComponents.SetActive(false);
                break;
            case gameplayState.enterName:
                //enable entername components only
                startScreenComponents.SetActive(false);
                optionsMenuComponents.SetActive(false);
                endScreenComponents.SetActive(false);
                EnterNameScreenComponents.SetActive(true);
                break;
            default:
                Debug.LogError("[GAME MANAGER] Warning! No gameplay state selected");
                break;
        }
    }
}
