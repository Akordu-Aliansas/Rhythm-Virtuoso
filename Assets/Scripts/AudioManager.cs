using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip songClip;
    public float songDelay = 2f; // Time before song starts after game load

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = songClip;
    }

    public void PlaySong()
    {
        audioSource.PlayDelayed(songDelay);
    }

    public float GetSongTime()
    {
        return audioSource.time;
    }
}