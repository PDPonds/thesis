using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    Music, SFX
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public SoundClass[] sounds;

    private void Awake()
    {
        Instance = this;

        GenerateAudioSource();

        DontDestroyOnLoad(gameObject);
    }

    void GenerateAudioSource()
    {
        foreach (SoundClass s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
    }

    public void Play(string name)
    {
        SoundClass s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        if (!s.source.isPlaying) s.source.Play();
    }

    public void Pause(string name)
    {
        SoundClass s = Array.Find(sounds, s => s.name == name);
        if (s == null) return;
        if (s.source.isPlaying) s.source.Play(); s.source.Pause();
    }

    public void PlayOnShot(string name)
    {
        SoundClass s = Array.Find(sounds, (sound) => sound.name == name);
        if (s == null) return;
        s.source.PlayOneShot(s.clip);
    }

}

[Serializable]
public class SoundClass
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup audioMixerGroup;
    [Range(0, 1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;
    public bool loop;
    [HideInInspector] public AudioSource source;
}
