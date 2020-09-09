using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    public AudioClip buttonClickSound;
    private Vector3 centerspot = new Vector3(0, 0, 0);
    //starts game
    public void StartGame()
    {
        AudioSource.PlayClipAtPoint(buttonClickSound, centerspot, GameManager.Instance.fxVolume);
        GameManager.Instance.gameState = GameManager.gameplayState.enterName;

        //if it's multiplayer
        if (GameManager.Instance.mulitplayer == true)
        {
            //set lives
            GameManager.Instance.playerOneLives = GameManager.Instance.playerStartingLives;
            GameManager.Instance.playerTwoLives = GameManager.Instance.playerStartingLives;
        }
        else
        {
            //set lives
            GameManager.Instance.playerOneLives = GameManager.Instance.playerStartingLives;
        }
    }
    //quits the game
    public void OnClickQuit()
    {
        AudioSource.PlayClipAtPoint(buttonClickSound, centerspot, GameManager.Instance.fxVolume);
        Application.Quit();
    }
    ///goes to option menu
    public void OnClickOptions()
    {
        AudioSource.PlayClipAtPoint(buttonClickSound, centerspot, GameManager.Instance.fxVolume);
        GameManager.Instance.gameState = GameManager.gameplayState.optionsMenu;
    }
  
}
