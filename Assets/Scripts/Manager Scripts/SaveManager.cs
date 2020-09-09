using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class SaveManager : Singleton<SaveManager>
{
    public Text titleText;
    public float musicVolume;
    public float fxVolume;
    public string top1Name, top2Name, top3Name, top4Name, top5Name;
    public float top1Score, top2Score, top3Score, top4Score, top5Score;
    public ScoreData emptyScoreData;

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString("TitleText", titleText.text);
        PlayerPrefs.SetFloat("MusicVolume", GameManager.Instance.musicVolume);
        PlayerPrefs.SetFloat("fxVolume", GameManager.Instance.fxVolume);
        if (GameManager.Instance.scoreDatas.Count > 0)
        {
            for (int i = 0; i < GameManager.Instance.scoreDatas.Count; i++)
            {
                PlayerPrefs.SetString("Top" + (i + 1) + "Name", GameManager.Instance.scoreDatas[i].playerName);
                PlayerPrefs.SetFloat("Top" + (i + 1) + "Score", GameManager.Instance.scoreDatas[i].score);
            }
        }
        PlayerPrefs.Save();
    }
    public void Load()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
            GameManager.Instance.musicVolume = musicVolume;
        }
        if (PlayerPrefs.HasKey("TitleText"))
        {
            titleText.text = PlayerPrefs.GetString("TitleText", titleText.text);
        }
        if (PlayerPrefs.HasKey("fxVolume"))
        {
            fxVolume = PlayerPrefs.GetFloat("fxVolume", fxVolume);
            GameManager.Instance.fxVolume = fxVolume;
        }


        if(PlayerPrefs.HasKey("Top1Name"))
        {
            Debug.Log("there's " + 1);
            //add place holder
            ScoreData tempScoreData = new ScoreData();
            //load and add the values into placeholder
            top1Score = PlayerPrefs.GetFloat("Top1Score");
            tempScoreData.score = top1Score;
            top1Name = PlayerPrefs.GetString("Top1Name");
            tempScoreData.playerName = top1Name;
            GameManager.Instance.scoreDatas.Add(tempScoreData);
        }
       
        if (PlayerPrefs.HasKey("Top2Name"))
        {
            Debug.Log("there's " + 2);
            //add place holder
            ScoreData tempScoreData = new ScoreData();
            //load and add the values into placeholder
            top2Score = PlayerPrefs.GetFloat("Top2Score");
            tempScoreData.score = top2Score;
            top2Name = PlayerPrefs.GetString("Top2Name");
            tempScoreData.playerName = top2Name;
            GameManager.Instance.scoreDatas.Add(tempScoreData);
        }
        if (PlayerPrefs.HasKey("Top3Name"))
        {
            Debug.Log("there's " + 3);
            //add place holder
            ScoreData tempScoreData = new ScoreData();
            //load and add the values into placeholder
            top3Score = PlayerPrefs.GetFloat("Top3Score");
            tempScoreData.score = top3Score;
            top3Name = PlayerPrefs.GetString("Top3Name");
            tempScoreData.playerName = top3Name;
            GameManager.Instance.scoreDatas.Add(tempScoreData);
        }
        if (PlayerPrefs.HasKey("Top4Name"))
        {
            Debug.Log("there's " + 4);
            //add place holder
            GameManager.Instance.scoreDatas.Add(emptyScoreData);
            //load and add the values into placeholder
            top4Score= PlayerPrefs.GetFloat("Top4Score");
            GameManager.Instance.scoreDatas[3].score = top4Score;
            top4Name = PlayerPrefs.GetString("Top4Name");
            GameManager.Instance.scoreDatas[3].playerName = top4Name;
        }
        if (PlayerPrefs.HasKey("Top5Name"))
        {
            Debug.Log("there's " + 5);
            //add place holder
            GameManager.Instance.scoreDatas.Add(emptyScoreData);
            //load and add the values into placeholder
            top5Score = PlayerPrefs.GetFloat("Top5Score");
            GameManager.Instance.scoreDatas[4].score = top5Score;
            top5Name = PlayerPrefs.GetString("Top5Name");
            GameManager.Instance.scoreDatas[4].playerName = top5Name;
        }
    }
    private void OnApplicationQuit()
    {
        Save();
    }
}
