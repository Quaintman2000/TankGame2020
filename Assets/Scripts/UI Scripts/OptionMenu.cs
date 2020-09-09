using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider fxSlider;
    public Slider playerNumSlider;
    public Toggle mapOfDayToggle;
    public AudioClip buttonClickSound;
    private Vector3 centerspot = new Vector3(0, 0, 0);

    private void OnEnable()
    {
        musicSlider.value = GameManager.Instance.musicVolume;
        fxSlider.value = GameManager.Instance.fxVolume;
    }
    //updates multiplayer value
    public void UpdateNumOfPlayers()
    {
        AudioSource.PlayClipAtPoint(buttonClickSound, centerspot, GameManager.Instance.fxVolume);
        //if two players is selected
        if (playerNumSlider.value == 2)
        {
            //set multiplayer true
            GameManager.Instance.mulitplayer = true;
        }
        else
        {
            //set multiplayer false
            GameManager.Instance.mulitplayer = false;
        }
    }
    //updates music volume
    public void UpdateMusicVolume()
    {
        GameManager.Instance.musicVolume = musicSlider.value;
    }
    //updates fx volume
    public void UpdateFxVolume()
    {

        GameManager.Instance.fxVolume = fxSlider.value;
    }
    //updates map of day value
    public void UpdateMapOfDayToggle()
    {
        AudioSource.PlayClipAtPoint(buttonClickSound, centerspot, GameManager.Instance.fxVolume);
        //if map of day toggle is toggled
        if (mapOfDayToggle.isOn)
        {
            //set it as true
            GameManager.Instance.MapOfDayToggle = true;
        }
        else
        {
            // set it as false
            GameManager.Instance.MapOfDayToggle = false;
        }
    }
    //goes back to start menu
    public void OnClickBack()
    {
        AudioSource.PlayClipAtPoint(buttonClickSound, centerspot, GameManager.Instance.fxVolume);
        GameManager.Instance.gameState = GameManager.gameplayState.startMenu;
    }
}
