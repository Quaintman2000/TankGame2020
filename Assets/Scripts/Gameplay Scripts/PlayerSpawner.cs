using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.playerSpawns.Add(this);
    }

    public void SpawnPlayer(GameObject player)
    {
        //spawn player at it's location
       GameObject newSpawn = Instantiate(player, this.gameObject.transform.position, this.gameObject.transform.rotation);

        //add player score data
       if(player == GameManager.Instance.player2Prefab)
        {
            newSpawn.GetComponent<PlayerScore>().playerScoreData.score = GameManager.Instance.playerTwoScore;
            newSpawn.GetComponent<PlayerScore>().playerScoreData.playerName = GameManager.Instance.PlayerTwoName;
        }
       else
        {
             newSpawn.GetComponent<PlayerScore>().playerScoreData.playerName = GameManager.Instance.playerOneName;
            newSpawn.GetComponent<PlayerScore>().playerScoreData.score = GameManager.Instance.playerOneScore;
        }
    }
 
}
