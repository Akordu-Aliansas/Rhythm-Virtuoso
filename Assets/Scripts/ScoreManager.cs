using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //public Text scoreText;  // The UI Text object that will display the score.
    private int score = 0;  // The player's score.

    // This function will be called to update the score.
    public void AddScore(int points)
    {
        score += points * ComboCounter.Instance.GetCurrentMultiplier();
        UpdateScoreDisplay();
    }

    // This function will update the score display on the UI.
    private void UpdateScoreDisplay()
    {
        //scoreText.text = "Score: " + score;  // Update the score on the UI.
        Debug.Log("Score: " + score);  // Output score to the console.
    }
}
