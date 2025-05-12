using TMPro;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    public static ComboCounter Instance { get; private set; }
    public TMP_Text comboDisplay;
    public TMP_Text multiplierDisplay;
    private int currentCombo = 0;
    public NoteSpawner spawner;
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
        comboDisplay.text = "Counter: " + currentCombo;
        //Debug.Log($"Combo increased! Current: {currentCombo}");  // New debug log
        OnComboChanged?.Invoke(currentCombo);

        // Optional: Add combo-based effects here (e.g., screen shake, sound)
    }

    public void ResetCombo(bool disableSpecial = false)
    {
        currentCombo = 0;
        comboDisplay.text = "Counter: " + currentCombo;
        multiplierDisplay.text = "Mult: " + 1 + "x";
        //Debug.Log("Combo reset to 0");  // New debug log
        OnComboChanged?.Invoke(currentCombo);

        // Optional: Add combo break effects here
    }

    // Call this from ScoreManager when calculating points
    public int GetCurrentMultiplier()
    {
        int multiplier = Mathf.Min(Mathf.FloorToInt(currentCombo / 10) + 1, 4);
        if (FindAnyObjectByType<StarPower>().isActive) multiplier *= 2;
        multiplierDisplay.text = "Mult: " + multiplier + "x";
        //Debug.Log($"Current multiplier: {multiplier}x");  // Optional multiplier log
        return multiplier;
    }
}