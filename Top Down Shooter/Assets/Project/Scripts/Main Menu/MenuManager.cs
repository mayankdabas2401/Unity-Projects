using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;

    public Slider[] volumeSliders;

    private void Start()
    {
        volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        volumeSliders[0].value = AudioManager.instance.musicVolumePercentage;
        volumeSliders[0].value = AudioManager.instance.sfxVolumePercent;
    }

    // On click Play Button
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    // On click Quit Button
    public void Quit()
    {
        Application.Quit();
    }

    // On click Option Button
    public void Options()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    // On click Back Button
    public void Back()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    // Master Volume Controller
    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    // Music Volume Controller
    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }

    // Sfx Volume Controller
    public void SetSfxVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }
}
