﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    /* CHANGES:
     * Singleton
     * Load music from Resources
     * Play music
     */

    public static MusicManager Instance { get; private set; }

    public enum Music
    {
        // TODO
    }

    private AudioSource audioSource;
    private Dictionary<Music, AudioClip> musicAudioClipDictionary;
    private float volume = 0.5f;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;

        musicAudioClipDictionary = new Dictionary<Music, AudioClip>();
        foreach (Music music in System.Enum.GetValues(typeof(Music)))
        {
            musicAudioClipDictionary[music] = Resources.Load<AudioClip>(music.ToString());
        }
    }

    public void PlayMusic(Music music, bool loopMusic)
    {
        audioSource.clip = musicAudioClipDictionary[music];
        if (loopMusic)
        {
            audioSource.loop = true;
        }
        else
        {
            audioSource.loop = false;
        }
        audioSource.Play();
    }

    public void IncreaseVolume()
    {
        volume += 0.1f;
        volume = Mathf.Clamp01(volume);
        audioSource.volume = volume;
    }

    public void DecreaseVolume()
    {
        volume -= 0.1f;
        volume = Mathf.Clamp01(volume);
        audioSource.volume = volume;
    }

    public float GetVolume()
    {
        return volume;
    }
}
