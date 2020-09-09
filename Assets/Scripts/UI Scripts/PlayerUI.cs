using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [Header("Player info:")]
    public int playerNum;

    [Header("Score stuff:")]
    public int score;
    public Text scoreText;

    [Header("Lives stuff:")]
    public Image[] livesImages;
    public int numberOfLives;

    private void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        //determine which player it is to determine number of lives & score to display
        if (playerNum == 1)
        {
            //grab player 1 info
            numberOfLives = GameManager.Instance.playerOneLives;
            score = GameManager.Instance.playerOneScore;
        }
        else if (playerNum == 2)
        {
            //grab player 2 info
            numberOfLives = GameManager.Instance.playerTwoLives;
            score = GameManager.Instance.playerTwoScore;
        }
        else
        {
            Debug.LogError("[PlayerUI] Incorrect player number set 34");
        }
        //active number of lives based on what gamemanager has
        for (int i = 0; i < livesImages.Length; i++)
        {
            if (i < numberOfLives)
            {
                livesImages[i].enabled = true;
            }
            else
            {
                livesImages[i].enabled = false;
            }
        }

        //update the score text
        scoreText.text = "Score: " + score.ToString();
    }


}
