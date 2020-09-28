using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public enum Sound
    {
        playerAttack,
        playerAttackAlt,
        playerDamaged,
        playerDamagedAlt,
        playerJump,
        playerJumpAlt,
        bullyAttack,
        bullyAttackAlt,
        bullyDamaged,
        bullyDamagedAlt,
        bullyDefault,
        bullyDie,
        bullyTaunt,
        bullyTauntAlt,
        maskLaugh
    }

    private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> soundAudioClipDictionary;
    private float volume = 1f;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();

        soundAudioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            soundAudioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }

    public void PlaySound(Sound sound, Vector3 position)
    {
        transform.position = position;
        audioSource.PlayOneShot(soundAudioClipDictionary[sound], volume);
    }

    public void PlaySound(Sound sound, Sound soundAlt, Vector3 position)
    {
        transform.position = position;

        int random01 = GetRandom01();
        switch (random01)
        {
            case 0:
                audioSource.PlayOneShot(soundAudioClipDictionary[sound], volume);
                break;
            case 1:
                audioSource.PlayOneShot(soundAudioClipDictionary[soundAlt], volume);
                break;
        }
    }

    public void IncreaseVolume()
    {
        volume += 0.1f;
        volume = Mathf.Clamp01(volume);
    }

    public void DecreaseVolume()
    {
        volume -= 0.1f;
        volume = Mathf.Clamp01(volume);
    }

    public float GetVolume()
    {
        return volume;
    }

    private int GetRandom01()
    {
        return Random.Range(0, 2);
    }
}