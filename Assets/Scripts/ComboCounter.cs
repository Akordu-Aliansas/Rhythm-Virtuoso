using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    public static ComboCounter Instance { get; private set; }

    private int currentCombo = 0;
    public int CurrentCombo => currentCombo; // Public read-only access

    // Event that triggers when combo changes (useful for future UI/sound effects)
    public event System.Action<int> OnComboChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void IncrementCombo()
    {
        currentCombo++;
        Debug.Log($"Combo increased! Current: {currentCombo}");  // New debug log
        OnComboChanged?.Invoke(currentCombo);

        // Optional: Add combo-based effects here (e.g., screen shake, sound)
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        Debug.Log("Combo reset to 0");  // New debug log
        OnComboChanged?.Invoke(currentCombo);

        // Optional: Add combo break effects here
    }

    // Call this from ScoreManager when calculating points
    public int GetCurrentMultiplier()
    {
        int multiplier = Mathf.FloorToInt(currentCombo / 10) + 1;
        Debug.Log($"Current multiplier: {multiplier}x");  // Optional multiplier log
        return multiplier;
    }
}