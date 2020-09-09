using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip titleMusic;
    public AudioClip backgroundMusic;
    public AudioSource AudioSource;

    // Update is called once per frame
    void Update()
    {
        AudioSource.volume = GameManager.Instance.musicVolume;
        //manage background and  title music
       switch(GameManager.Instance.gameState)
        {
            case GameManager.gameplayState.startMenu:
                AudioSource.clip = titleMusic;
                //check if it's playing
                if(!(AudioSource.isPlaying))
                {
                    AudioSource.Play();
                }
                break;
            case GameManager.gameplayState.gameRunning:
                AudioSource.clip = backgroundMusic;
                //check if it's playing
                if (!(AudioSource.isPlaying))
                {
                    AudioSource.Play();
                }
                break;
            case GameManager.gameplayState.optionsMenu:
                AudioSource.clip = titleMusic;
                //check if it's playing
                if (!(AudioSource.isPlaying))
                {
                    AudioSource.Play();
                }
                break;
            case GameManager.gameplayState.enterName:
                AudioSource.clip = titleMusic;
                //check if it's playing
                if (!(AudioSource.isPlaying))
                {
                    AudioSource.Play();
                }
                break;
            case GameManager.gameplayState.endScreen:
                AudioSource.clip = backgroundMusic;
                //check if it's playing
                if (!(AudioSource.isPlaying))
                {
                    AudioSource.Play();
                }
                break;
            default:
                AudioSource.Stop();
                break;
        }    
    }
}
