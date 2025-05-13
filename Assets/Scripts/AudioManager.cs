using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music Settings")]
    public AudioMixerGroup mixer;
    public AudioClip songClip;
    public TickRate tickRate;
    private AudioSource musicSource;

    [Header("Sound Effects")]
    public AudioClip hitSound;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Set up music source
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = songClip;
            musicSource.outputAudioMixerGroup = mixer;

            // Set up SFX source
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySong()
    {
        StartCoroutine(StartSongWithDelay());
    }

    private IEnumerator StartSongWithDelay()
    {
        yield return new WaitForSeconds(tickRate.waitTime - Time.timeSinceLevelLoad);
        musicSource.Play();
    }

    // Call this when hitting a note
    public void PlayHitSound()
    {
        if (hitSound != null)
        {
            sfxSource.PlayOneShot(hitSound);
        }
    }

    public float GetSongTime()
    {
        return musicSource.time;
    }

    public void PauseSong()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();
    }

    public void ResumeSong()
    {
        if (!musicSource.isPlaying)
            musicSource.UnPause();
    }

}