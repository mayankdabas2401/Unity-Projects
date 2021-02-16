using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    [System.Serializable]
    public class SoundGroup
    {
        public string groupName;
        public AudioClip[] audioClips;
    }

    public SoundGroup[] soundGroups;

    Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();
    private void Awake()
    {
        foreach(SoundGroup group in soundGroups)
        {
            groupDictionary.Add(group.groupName, group.audioClips);
        }
    }

    public AudioClip GetClip(string clipName)
    {
        if(groupDictionary.ContainsKey(clipName))
        {
            AudioClip[] clips = groupDictionary[clipName];
            return clips[Random.Range(0, clips.Length)];
        }
        return null;
    }
}
