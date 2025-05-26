using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public FadeInOut fade;
    public ChangeSelectedSong changeSong;

    private void Start()
    {
        changeSong = FindAnyObjectByType<ChangeSelectedSong>();
        fade = FindAnyObjectByType<FadeInOut>();
    }

    public void ChangeToGameScene(int id)
    {
        changeSong.selectedSong = id;
        StartCoroutine(_ChangeToGameScene());
    }

    private IEnumerator _ChangeToGameScene()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("InGame");
    }

    // Fixed: Add small delay for UI sound to play
    public void ChangeToOptionScene()
    {
        StartCoroutine(DelayedSceneChange("OptionsMenu"));
    }

    public void ChangeToSelectScene()
    {
        StartCoroutine(DelayedSceneChange("SongSelect"));
    }

    public void ChangeToMainMenuScene()
    {
        StartCoroutine(DelayedSceneChange("MainMenu"));
    }

    public void ChangeToHighScoreScene()
    {
        StartCoroutine(DelayedSceneChange("ScoringBoard"));
    }

    // Generic method for delayed scene changes
    private IEnumerator DelayedSceneChange(string sceneName)
    {
        yield return new WaitForSeconds(0.25f); // Small delay for UI sound
        SceneManager.LoadScene(sceneName);
    }

    public void doExitGame()
    {
        Application.Quit();
    }
}