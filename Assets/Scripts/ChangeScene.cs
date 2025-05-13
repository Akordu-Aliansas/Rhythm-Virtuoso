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
    public void ChangeToOptionScene()
    {
        SceneManager.LoadScene("OptionsMenu");
    }
    public void ChangeToSelectScene()
    {
        SceneManager.LoadScene("SongSelect");
    }
    public void ChangeToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ChangeToHighScoreScene()
    {
        SceneManager.LoadScene("ScoringBoard");
    }
    public void doExitGame()
    {
        Application.Quit();
    }
}
