using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel
    {
        Master, Sfx, Music
    }

    public static AudioManager instance;

    public float masterVolumePercent { get; private set; }
    public float sfxVolumePercent { get; private set; }
    public float musicVolumePercentage { get; private set; }

    private AudioSource sfx2DSounds;
    private AudioSource[] backgroundMusic;
    private Transform audioListener;
    private Transform playerTransform;
    private AudioLibrary library;

    private int activeMusicSourceIndex;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            backgroundMusic = new AudioSource[2];

            for (int i = 0; i < 2; i++)
            {
                GameObject newBGMusic = new GameObject(
                        "Background Music" + (i + 1)
                    );
                backgroundMusic[i] = newBGMusic.AddComponent<AudioSource>();
                newBGMusic.transform.parent = transform;
            }

            GameObject newSfx2DSound = new GameObject("2D Sfx");
            sfx2DSounds = newSfx2DSound.AddComponent<AudioSource>();
            newSfx2DSound.transform.parent = transform;

            library = GetComponent<AudioLibrary>();
            audioListener = FindObjectOfType<AudioListener>().transform;
            
            if(FindObjectOfType<Player>() != null)
                playerTransform = FindObjectOfType<Player>().transform;

            masterVolumePercent = PlayerPrefs.GetFloat("master volume", 0.5f);
            sfxVolumePercent = PlayerPrefs.GetFloat("sfx volume", 1f);
            musicVolumePercentage = PlayerPrefs.GetFloat("music volume", 0.5f);
        }
    }

    private void Update()
    {
        if(playerTransform != null)
        {
            audioListener.position = playerTransform.position;
        }
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch(channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercentage = volumePercent;
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                break;
        }

        backgroundMusic[0].volume = musicVolumePercentage * masterVolumePercent;
        backgroundMusic[1].volume = musicVolumePercentage * masterVolumePercent;

        PlayerPrefs.SetFloat("master volume", masterVolumePercent);
        PlayerPrefs.SetFloat("sfx volume", sfxVolumePercent);
        PlayerPrefs.SetFloat("music volume", musicVolumePercentage);
        PlayerPrefs.Save();
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1f)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        backgroundMusic[activeMusicSourceIndex].clip = clip;
        backgroundMusic[activeMusicSourceIndex].Play();

        StartCoroutine(FadeRoutine(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        if(clip != null)
        {
            AudioSource.PlayClipAtPoint(
                clip,
                position,
                sfxVolumePercent * masterVolumePercent
            );
        }    
        
    }

    public void PlaySound(string clipName, Vector3 position)
    {
        PlaySound(library.GetClip(clipName), position);
    }

    public void Play2DSound(string clipName)
    {
        sfx2DSounds.PlayOneShot(library.GetClip(clipName), sfxVolumePercent * masterVolumePercent);
    }

    IEnumerator FadeRoutine(float fadeDuration)
    {
        float percent = 0f;

        while(percent < 1f)
        {
            percent += Time.deltaTime * 1 / fadeDuration;

            backgroundMusic[activeMusicSourceIndex].volume = Mathf.Lerp(
                    0,
                    musicVolumePercentage * masterVolumePercent,
                    percent
                );
            backgroundMusic[1 - activeMusicSourceIndex].volume = Mathf.Lerp(
                    musicVolumePercentage * masterVolumePercent,
                    0,
                    percent
                );
            yield return null;
        }
    }
}
