using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField] private Sound[] sfx;
    [SerializeField] private Sound[] music;

    public static AudioManager instance;
    private static Sound currentMusic;

    public static AudioManager Instance {
        get {
            if (instance == null)
                instance = new AudioManager();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sfx) {
            s.Setup(gameObject.AddComponent<AudioSource>());
        }
        foreach (Sound s in music) {
            s.Setup(gameObject.AddComponent<AudioSource>());
        }

    }

    public void PlaySFX(string name) {
        Sound s = Array.Find(sfx, sound => sound.GetName() == name);
        if (s == null) Debug.LogWarning("No sound found with name: " + name);
        else s.Play();
    }
    public void PlayMusic(string name) {
        Sound s = Array.Find(music, sound => sound.GetName() == name);
        if (s == null) Debug.LogWarning("No sound found with name: " + name);
        else {
            if (currentMusic != s) {
                if (currentMusic != null) currentMusic.Stop();
                currentMusic = s;
                s.Play();
            }
        }
    }
}
