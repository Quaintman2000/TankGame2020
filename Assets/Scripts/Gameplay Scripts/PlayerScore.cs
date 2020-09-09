using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public ScoreData playerScoreData;

   public void AddScoreToHighScores()
    {
        GameManager.Instance.scoreDatas.Add(playerScoreData);
    }

    
    private void OnDestroy()
    {
        //if this is player one
        if(this.gameObject.GetComponent<TankData>() == GameManager.Instance.playerOneData)
        {
            //add to highscore once the player one has ran out of lives
            if(GameManager.Instance.playerOneLives == 0)
            {
                AddScoreToHighScores();
            }
        }
        else
        {
            //add to highscore once the player two has ran out of lives
            if (GameManager.Instance.playerTwoLives == 0)
            {
                AddScoreToHighScores();
            }
        }
    }
}
