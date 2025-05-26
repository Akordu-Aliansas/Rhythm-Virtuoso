using UnityEngine;
using UnityEngine.UI;

public class SongProgressBar : MonoBehaviour
{
    [Header("Progress Bar UI")]
    public Image progressFill; // Drag your Handle here directly
    public Text timeText;

    [Header("Settings")]
    public bool showTimeRemaining = false;

    void Start()
    {
        // Auto-find if not assigned
        if (progressFill == null)
        {
            // Look for Handle image in scrollbar
            Scrollbar scrollbar = GetComponentInChildren<Scrollbar>();
            if (scrollbar != null)
            {
                Transform handle = scrollbar.transform.Find("Sliding Area/Handle");
                if (handle != null)
                {
                    progressFill = handle.GetComponent<Image>();
                }
            }
        }

        // Configure the fill image
        if (progressFill != null)
        {
            progressFill.type = Image.Type.Filled;
            progressFill.fillMethod = Image.FillMethod.Horizontal;
            progressFill.fillOrigin = 0;
            progressFill.fillAmount = 0f;

            // Disable scrollbar interaction
            Scrollbar scrollbar = GetComponentInChildren<Scrollbar>();
            if (scrollbar != null)
            {
                scrollbar.interactable = false;
            }

            Debug.Log("Progress bar ready!");
        }
    }

    void Update()
    {
        if (progressFill != null &&
            AudioManager.Instance != null &&
            AudioManager.Instance.MusicSource != null &&
            AudioManager.Instance.MusicSource.isPlaying)
        {
            UpdateProgress();
        }
    }

    void UpdateProgress()
    {
        var musicSource = AudioManager.Instance.MusicSource;

        if (musicSource.clip != null)
        {
            // Calculate and apply progress
            float progress = musicSource.time / musicSource.clip.length;
            progressFill.fillAmount = Mathf.Clamp01(progress);

            // Update time text
            if (timeText != null)
            {
                if (showTimeRemaining)
                {
                    float remaining = musicSource.clip.length - musicSource.time;
                    timeText.text = $"-{FormatTime(remaining)}";
                }
                else
                {
                    timeText.text = $"{FormatTime(musicSource.time)} / {FormatTime(musicSource.clip.length)}";
                }
            }
        }
    }

    string FormatTime(float seconds)
    {
        int mins = Mathf.FloorToInt(seconds / 60);
        int secs = Mathf.FloorToInt(seconds % 60);
        return $"{mins:00}:{secs:00}";
    }
}