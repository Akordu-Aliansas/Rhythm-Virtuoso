using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;  // The UI Text object that will display the score.
    private float score = 0;  // The player's score.

    // Public getter for other scripts:
    public int CurrentScore => Mathf.RoundToInt(score);

    // This function will be called to update the score.
    public void AddScore(float points)
    {
        score += points * ComboCounter.Instance.GetCurrentMultiplier();
        UpdateScoreDisplay();
    }

    // This function will update the score display on the UI.
    private void UpdateScoreDisplay()
    {
        scoreText.text = "Score: " + Mathf.Round(score); // Update the score on the UI.
        //Debug.Log("Score: " + score);  // Output score to the console.
    }
}
