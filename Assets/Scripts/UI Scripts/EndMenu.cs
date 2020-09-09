using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    public AudioClip buttonClickSound;
    public Text textBoard;
    private const int maxHighScoresNum = 5;
    private Vector3 centerspot = new Vector3(0, 0, 0);

    public void OnClickRestart()
    {
        //play button sound
        AudioSource.PlayClipAtPoint(buttonClickSound, centerspot, GameManager.Instance.fxVolume);
        GameManager.Instance.gameState = GameManager.gameplayState.startMenu;
    }

    public void OnClickQuit()
    {
        //play button sound
        AudioSource.PlayClipAtPoint(buttonClickSound, centerspot, GameManager.Instance.fxVolume);
        Application.Quit();
    }

    private void OnEnable()
    {
        GameManager.Instance.scoreDatas.Sort();
        GameManager.Instance.scoreDatas.Reverse();
        if (GameManager.Instance.scoreDatas.Count > 5)
        {
            GameManager.Instance.scoreDatas = GameManager.Instance.scoreDatas.GetRange(index: 0, count: maxHighScoresNum);
        }
        for (int i = 0; i < GameManager.Instance.scoreDatas.Count; i++)
        {
            textBoard.text += (i + 1) + ") " + GameManager.Instance.scoreDatas[i].playerName + ": " + GameManager.Instance.scoreDatas[i].score + " Points!" + "\n";
        }
    }
    private void OnDisable()
    {
        textBoard.text = string.Empty;
    }
}
