using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSplitter : Singleton<CameraSplitter>
{
    public Camera[] playerCameras = new Camera[2];
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState == GameManager.gameplayState.gameRunning)
        {
            if (GameManager.Instance.playerOneData != null && GameManager.Instance.playerTwoData != null)
            {
                //grab cameras
                playerCameras[0] = GameManager.Instance.playerOneData.GetComponentInChildren<Camera>();
                playerCameras[1] = GameManager.Instance.playerTwoData.GetComponentInChildren<Camera>();

                //set camera sizes for splitscreen
                playerCameras[0].rect = new Rect(x: 0, y: 0, width: 0.5f, height: 1f);
                playerCameras[1].rect = new Rect(x: 0.5f, y: 0, width: 0.5f, height: 1f);
            }
            else
            {
                //if there is a player one
                if (GameManager.Instance.playerOneData != null)
                {
                    //grab camera
                    playerCameras[0] = GameManager.Instance.playerOneData.GetComponentInChildren<Camera>();

                    //set camera rect
                    playerCameras[0].rect = new Rect(x: 0, y: 0, width: 1f, height: 1f);
                }
                //if there is a player two
                if (GameManager.Instance.playerTwoData != null)
                {
                    //grab camera
                    playerCameras[1] = GameManager.Instance.playerTwoData.GetComponentInChildren<Camera>();

                    //set camera rect
                    playerCameras[1].rect = new Rect(x: 0, y: 0, width: 1f, height: 1f);
                }
            }
        }
    }
}
