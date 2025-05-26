using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class KeyRebindManager : MonoBehaviour
{

    public TextMeshProUGUI conflictWarningText;
    // UI elements for showing current key bindings
    public TextMeshProUGUI lane1KeyText;
    public TextMeshProUGUI lane2KeyText;
    public TextMeshProUGUI lane3KeyText;
    public TextMeshProUGUI lane4KeyText;
    public TextMeshProUGUI lane5KeyText;

    // Rebind buttons for each lane
    public Button lane1RebindButton;
    public Button lane2RebindButton;
    public Button lane3RebindButton;
    public Button lane4RebindButton;
    public Button lane5RebindButton;

    private Coroutine warningFadeCoroutine;
    private bool waitingForKey = false;
    private int laneToRebind = -1;

    void Start()
    {
        UpdateKeyTextDisplay();

        conflictWarningText.text = "";
        conflictWarningText.alpha = 0f; // Force it fully invisible at start

        lane1RebindButton.onClick.AddListener(() => BeginRebind(0));
        lane2RebindButton.onClick.AddListener(() => BeginRebind(1));
        lane3RebindButton.onClick.AddListener(() => BeginRebind(2));
        lane4RebindButton.onClick.AddListener(() => BeginRebind(3));
        lane5RebindButton.onClick.AddListener(() => BeginRebind(4));
    }


    void BeginRebind(int laneIndex)
    {
        conflictWarningText.text = "";
        laneToRebind = laneIndex;
        waitingForKey = true;

        // Show rebind prompt
        GetKeyTextForLane(laneIndex).text = "Press any key...";
    }

    void Update()
    {
        if (!waitingForKey) return;

        Key pressedKey = GetPressedKey();
        if (pressedKey == Key.None) return;

        if (pressedKey == Key.Escape)
        {
            CancelRebind();
            return;
        }

        // Save and apply
        SetLaneKey(laneToRebind, pressedKey);
    }

    void SetLaneKey(int laneIndex, Key newKey)
    {
        if (IsKeyAlreadyUsed(newKey, laneIndex))
        {
            ShowConflictWarning($"Key '{newKey}' is already in use!");
            Debug.LogWarning(conflictWarningText.text);
            CancelRebind();
            return;
        }


        waitingForKey = false;
        string prefKey = $"Lane{laneIndex}Key";
        PlayerPrefs.SetString(prefKey, newKey.ToString());
        PlayerPrefs.Save();

        conflictWarningText.text = ""; // Clear warning if successful
        UpdateKeyTextDisplay();
    }

    bool IsKeyAlreadyUsed(Key key, int currentLane)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == currentLane) continue;

            string otherPrefKey = $"Lane{i}Key";
            string saved = PlayerPrefs.GetString(otherPrefKey, GetDefaultKey(i).ToString());

            if (System.Enum.TryParse<Key>(saved, out Key otherKey))
            {
                if (otherKey == key)
                    return true;
            }
        }
        return false;
    }

    void ShowConflictWarning(string message)
    {
        conflictWarningText.text = message;
        conflictWarningText.alpha = 1f;

        // If a previous fade is running, stop it
        if (warningFadeCoroutine != null)
        {
            StopCoroutine(warningFadeCoroutine);
        }
        warningFadeCoroutine = StartCoroutine(FadeOutWarning());
    }


    Key GetDefaultKey(int laneIndex)
    {
        return laneIndex switch
        {
            0 => Key.A,
            1 => Key.S,
            2 => Key.D,
            3 => Key.F,
            4 => Key.G,
            _ => Key.None
        };
    }

    void CancelRebind()
    {
        waitingForKey = false;
        UpdateKeyTextDisplay();
    }

    void UpdateKeyTextDisplay()
    {
        lane1KeyText.text = GetDisplayNameFromPrefs("Lane0Key", Key.A);
        lane2KeyText.text = GetDisplayNameFromPrefs("Lane1Key", Key.S);
        lane3KeyText.text = GetDisplayNameFromPrefs("Lane2Key", Key.D);
        lane4KeyText.text = GetDisplayNameFromPrefs("Lane3Key", Key.F);
        lane5KeyText.text = GetDisplayNameFromPrefs("Lane4Key", Key.G);
    }

    string GetDisplayNameFromPrefs(string prefKey, Key fallback)
    {
        string saved = PlayerPrefs.GetString(prefKey, fallback.ToString());
        if (System.Enum.TryParse<Key>(saved, out Key parsed) && Keyboard.current[parsed] != null)
        {
            return Keyboard.current[parsed].displayName;
        }
        return fallback.ToString();
    }

    TextMeshProUGUI GetKeyTextForLane(int laneIndex)
    {
        return laneIndex switch
        {
            0 => lane1KeyText,
            1 => lane2KeyText,
            2 => lane3KeyText,
            3 => lane4KeyText,
            4 => lane5KeyText,
            _ => null
        };
    }

    Key GetPressedKey()
    {
        // Check only valid, physical keys instead of all enum values
        var keyboard = Keyboard.current;
        if (keyboard == null) return Key.None;

        // Check all physical keys on the keyboard
        foreach (var key in keyboard.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                return key.keyCode;
            }
        }

        return Key.None;
    }
    IEnumerator FadeOutWarning()
    {
        yield return new WaitForSeconds(2f); // Wait before starting fade (2 seconds)

        float duration = 1f; // Fade out over 1 second
        float elapsed = 0f;
        Color originalColor = conflictWarningText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            conflictWarningText.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                Mathf.Lerp(1f, 0f, elapsed / duration)
            );
            yield return null;
        }

        conflictWarningText.text = "";
    }
}
