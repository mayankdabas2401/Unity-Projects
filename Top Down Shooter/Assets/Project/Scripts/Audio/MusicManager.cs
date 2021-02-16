using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme;
    public AudioClip menuTheme;

    private string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        OnLevelWasLoaded(0);
    }

    private void OnLevelWasLoaded(int level)
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if(sceneName != newSceneName)
        {
            sceneName = newSceneName;
            Invoke("PlayMusic", 0.2f);
        }
    }

    private void PlayMusic()
    {
        AudioClip clipToPlay = null;

        if(sceneName == "Menu")
        {
            clipToPlay = menuTheme;
        }
        else if(sceneName == "Game")
        {
            clipToPlay = mainTheme;
        }

        if(clipToPlay != null)
        {
            AudioManager.instance.PlayMusic(clipToPlay, 2);
            Invoke("PlayMusic", clipToPlay.length);
        }
    }
}
