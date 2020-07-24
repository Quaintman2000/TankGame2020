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

    private void Start()
    {
        Load();
    }
    public void Save()
    {
        PlayerPrefs.SetString("TitleText", titleText.text);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("fxVolume", fxVolume);
        PlayerPrefs.Save();
    }
    public void Load()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
        }
        if (PlayerPrefs.HasKey("TitleText"))
        {
            titleText.text = PlayerPrefs.GetString("TitleText", titleText.text);
        }
        if (PlayerPrefs.HasKey("fxVolume"))
        {
            fxVolume = PlayerPrefs.GetFloat("fxVolume", fxVolume);
        }
    }
    private void OnApplicationQuit()
    {
        Application.Quit();
    }
}
