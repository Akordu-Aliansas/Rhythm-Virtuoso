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

        // If you forgot to drag it in, fall back to the singleton:
        if (audioManager == null)
            audioManager = AudioManager.Instance;

        // Now grab the AudioSource that was added at runtime:
        musicSource = audioManager.MusicSource;

        retryButton.onClick.AddListener(OnRetry);
        menuButton.onClick.AddListener(OnMenu);
    }

    void Update()
    {
        if (hasEnded || musicSource == null)
            return;

        if (!songStarted && musicSource.isPlaying)
            songStarted = true;

        if (songStarted && !musicSource.isPlaying)
            ShowEndScreen();
    }

    private void ShowEndScreen()
    {
        hasEnded = true;
        Time.timeScale = 0f;

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
