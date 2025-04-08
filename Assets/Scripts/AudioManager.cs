using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioClip songClip;
    public TickRate tickRate;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = songClip;
    }

    public void PlaySong()
    {
        StartCoroutine(_PlaySong());
    }
    private IEnumerator _PlaySong()
    {
        yield return new WaitForSeconds(tickRate.waitTime - Time.timeSinceLevelLoad);
        audioSource.Play();
    }

    public float GetSongTime()
    {
        return audioSource.time;
    }
}