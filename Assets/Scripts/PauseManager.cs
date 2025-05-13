using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Tooltip("Assign your Pause Menu Canvas (inactive by default)")]
    public GameObject pauseMenuUI;

    [Tooltip("Drag in all of your InputManager components here (one per lane)")]
    public InputManager[] inputManagers;

    [Tooltip("Drag your in-scene AudioManager here")]
    public AudioManager audioMgr;

    [Tooltip("Assign your End-Screen UI panel here")]
    public GameObject endScreenUI;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // if the song has just ended and the end-screen is up, ignore Escape
        if (endScreenUI != null && endScreenUI.activeSelf)
            return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // freeze/unfreeze time & UI
        Time.timeScale = isPaused ? 0f : 1f;
        pauseMenuUI.SetActive(isPaused);

        // pause/resume audio
        if (isPaused) audioMgr.PauseSong();
        else audioMgr.ResumeSong();

        // disable/enable every InputManager
        foreach (var im in inputManagers)
            if (im != null)
                im.enabled = !isPaused;
    }

    // Called by your ÅgResumeÅh button in the pause UI
    public void OnResumeButton()
    {
        if (isPaused) TogglePause();
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;            // make sure timeÅfs back to normal
        SceneManager.LoadScene("MainMenu");  // your menu scene name
    }

}
