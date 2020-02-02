using UnityEngine;
using System.Collections.Generic;

public class GameSounds : MonoBehaviour {

    public static GameSounds instance;

    [System.Serializable]
    public class SoundFiles {
        public AudioClip beep1, beep2;
    }
    public SoundFiles sounds; 
    private List<AudioSource> sources;
    public AudioSource sourcePrefab;
    
    void Awake() {
        sources = new List<AudioSource>();
        instance = this;
    }
    
    public void PlayClip(AudioClip clip, float volume = 1) {
        AudioSource source = null;
        for (int i = 0; i < sources.Count; i++) {
            if (!sources[i].isPlaying) {
                source = sources[i];
                break;
            }
        }
        if (!source) {
            source = Instantiate(sourcePrefab);
            source.transform.SetParent(transform);
            source.playOnAwake = false;
            sources.Add(source);
        }
        source.clip = clip;
        source.volume = volume;
        source.Play();
    }

    public void PlayBeep1()
    {
        PlayClip(sounds.beep1);
    }

    public void PlayBeep2()
    {
        PlayClip(sounds.beep2);
    }

}