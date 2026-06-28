using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsManager : MonoBehaviour
{
    private int totalIndex = 0;
    [SerializeField] private SoundEffectsLists soundEffects;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        totalIndex = soundEffects.punchHitSoundEffects.Count;

        if(_audioSource != null)
        {
            _audioSource.clip = soundEffects.punchHitSoundEffects[0];
        }
    }

    private AudioClip GetRandomPunchAudio()
    {
        int index;
        index = UnityEngine.Random.Range(0, totalIndex);

        return soundEffects.punchHitSoundEffects[index];
    }

    public void PlayPunchAudio()
    {
        AudioClip punchAudio = GetRandomPunchAudio();

        _audioSource.clip = punchAudio;
        _audioSource.Play();
    }

    public void PlayCriticalHitAudio()
    {
        _audioSource.clip = soundEffects.criticalHitSoundEffect;
        _audioSource.Play();
    }

    public void PlayParryHitAudio()
    {
        _audioSource.clip = soundEffects.parryHitSoundEffect;
        _audioSource.Play();
    }

    public void PlayStunnedAudio()
    {
        _audioSource.clip = soundEffects.stunnedSoundEffect;
        _audioSource.Play();
    }
}

[System.Serializable]
public struct SoundEffectsLists
{
    public List<AudioClip> punchHitSoundEffects;
    public AudioClip criticalHitSoundEffect;
    public AudioClip parryHitSoundEffect;
    public AudioClip stunnedSoundEffect;
}
