using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject endScreenUI;
    public TMP_Text headerText;
    public TMP_Text scoreText;
    public Button retryButton;
    public Button menuButton;

    [Header("Gameplay References")]
    [Tooltip("Drag your AudioManager GameObject here")]
    public AudioManager audioManager;
    [Tooltip("Your ScoreManager component")]
    public ScoreManager scoreManager;

    private AudioSource musicSource;
    private bool songStarted = false;
    private bool hasEnded = false;

    void Start()
    {
        endScreenUI.SetActive(false);
        if (audioManager == null)
            audioManager = AudioManager.Instance;

        musicSource = audioManager.MusicSource;

        retryButton.onClick.AddListener(OnRetry);
        menuButton.onClick.AddListener(OnMenu);
    }

    void Update()
    {

        if (musicSource == null && audioManager != null && audioManager.MusicSource != null)
        {
            musicSource = audioManager.MusicSource;
        }

        if (hasEnded || musicSource == null || musicSource.clip == null)
            return;

        if (!songStarted && musicSource.isPlaying)
            songStarted = true;

        
        if (songStarted
            && musicSource.time >= musicSource.clip.length - 0.05f
            && Time.timeScale > 0f)
        {
            ShowEndScreen();
        }
    }

    private void ShowEndScreen()
    {
        hasEnded = true;
        Time.timeScale = 0f;

        GetComponent<HighScoreUpdate>().AddNewEntry(scoreManager.CurrentScore);

        headerText.text = "Song Complete!";
        scoreText.text = $"Score: {scoreManager.CurrentScore}";

        endScreenUI.SetActive(true);
    }

    private void OnRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
