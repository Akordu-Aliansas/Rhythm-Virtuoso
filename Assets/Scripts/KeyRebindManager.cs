using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class KeyRebindManager : MonoBehaviour
{
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

    private bool waitingForKey = false;
    private int laneToRebind = -1;

    void Start()
    {
        UpdateKeyTextDisplay();

        // Assign rebind button actions
        lane1RebindButton.onClick.AddListener(() => BeginRebind(0));
        lane2RebindButton.onClick.AddListener(() => BeginRebind(1));
        lane3RebindButton.onClick.AddListener(() => BeginRebind(2));
        lane4RebindButton.onClick.AddListener(() => BeginRebind(3));
        lane5RebindButton.onClick.AddListener(() => BeginRebind(4));
    }

    void BeginRebind(int laneIndex)
    {
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
        waitingForKey = false;
        string prefKey = $"Lane{laneIndex}Key";

        PlayerPrefs.SetString(prefKey, newKey.ToString());
        PlayerPrefs.Save();

        UpdateKeyTextDisplay();
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
        foreach (Key key in System.Enum.GetValues(typeof(Key)))
        {
            if (key == Key.None) continue;
            if (Keyboard.current[key] != null && Keyboard.current[key].wasPressedThisFrame)
            {
                return key;
            }
        }
        return Key.None;
    }
}
