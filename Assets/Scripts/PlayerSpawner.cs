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
        Instantiate(player, this.gameObject.transform.position, this.gameObject.transform.rotation);
    }
}
