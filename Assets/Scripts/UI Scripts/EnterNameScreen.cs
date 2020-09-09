using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterNameScreen : MonoBehaviour
{
    public InputField player1TextBox;
    public GameObject player2NameLabel;
    public InputField player2TextBox;
    public AudioClip buttonClickSound;
    private Vector3 centerspot = new Vector3(0, 0, 0);

    private void OnEnable()
    {
        //check to see if it's multiplayer to see if the game should ask for player two's name
        if(GameManager.Instance.mulitplayer == false)
        {
            player2NameLabel.SetActive(false);
            player2TextBox.gameObject.SetActive(false);
        }
        else
        {
            player2NameLabel.SetActive(true);
            player2TextBox.gameObject.SetActive(true);
        }
    }

    public void OnClickSubmit()
    {
        //play button sound
        AudioSource.PlayClipAtPoint(buttonClickSound, centerspot, GameManager.Instance.fxVolume);
        //submit the names the player's have chosen in the textboxs into the gamemanager to hold
        //check to see if they typed a name, if not then use the player 1/player 2 place holder
        if(player1TextBox.text == "")
        {
            GameManager.Instance.playerOneName = "Player 1";
        }
        else
        {
            GameManager.Instance.playerOneName = player1TextBox.text;
        }
        //do the same for player 2
        if(GameManager.Instance.mulitplayer == true)
        {
            if (player2TextBox.text == "")
            {
                GameManager.Instance.PlayerTwoName = "Player 2";
            }
            else
            {
                GameManager.Instance.PlayerTwoName = player2TextBox.text;
            }
        }
        GameManager.Instance.playerOneScore = 0;
        GameManager.Instance.playerTwoScore = 0;
        //start the game
        GameManager.Instance.gameState = GameManager.gameplayState.gameRunning;
        //if there's no map gen
        if (GameManager.Instance.mapGenerator == null)
        {
            //spawn a map generator
            GameManager.Instance.mapGenerator = Instantiate(GameManager.Instance.mapGeneratorPrefab);

        }
    }
}
