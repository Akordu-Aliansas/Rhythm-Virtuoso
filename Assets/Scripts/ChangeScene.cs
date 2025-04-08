using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    FadeInOut fade;
    private void Start()
    {
        fade = FindAnyObjectByType<FadeInOut>();
    }
    public void ChangeToGameScene()
    {
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
}
