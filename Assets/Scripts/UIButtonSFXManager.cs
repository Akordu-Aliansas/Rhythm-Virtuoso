using UnityEngine;

public class UIButtonSFXManager : MonoBehaviour
{
    [Header("Global UI SFX Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip defaultButtonClickSFX;
    [SerializeField] private AudioClip confirmButtonSFX;
    [SerializeField] private AudioClip cancelButtonSFX;
    [SerializeField] private float defaultVolume = 1f;

    public static UIButtonSFXManager Instance { get; private set; }

    void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            // Another instance exists, copy its settings to this one and destroy the old one
            Debug.Log("Replacing existing UI_SFX_Manager with new one");

            // Copy audio settings from existing instance
            var oldInstance = Instance;
            if (defaultButtonClickSFX == null && oldInstance.defaultButtonClickSFX != null)
                defaultButtonClickSFX = oldInstance.defaultButtonClickSFX;
            if (confirmButtonSFX == null && oldInstance.confirmButtonSFX != null)
                confirmButtonSFX = oldInstance.confirmButtonSFX;
            if (cancelButtonSFX == null && oldInstance.cancelButtonSFX != null)
                cancelButtonSFX = oldInstance.cancelButtonSFX;

            // Destroy the old instance
            Destroy(oldInstance.gameObject);
        }

        // Set this as the new instance
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create audio source if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.Log("Creating new AudioSource component");
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Ensure proper AudioSource settings
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = 1f;

        Debug.Log($"AudioSource setup complete: {audioSource != null}");

        Debug.Log("UI_SFX_Manager created and persisted");
    }

    void OnDestroy()
    {
        Debug.Log("UI_SFX_Manager destroyed!");
        // Only clear instance if this is the current instance
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void PlayButtonSFX(ButtonSFXType sfxType = ButtonSFXType.Default, float volume = -1f)
    {
        Debug.Log($"PlayButtonSFX called with type: {sfxType}");
        Debug.Log($"Instance exists: {Instance != null}");
        Debug.Log($"AudioSource exists: {audioSource != null}");

        if (audioSource == null)
        {
            Debug.LogError("AudioSource is null!");
            return;
        }

        AudioClip clipToPlay = sfxType switch
        {
            ButtonSFXType.Confirm => confirmButtonSFX ?? defaultButtonClickSFX,
            ButtonSFXType.Cancel => cancelButtonSFX ?? defaultButtonClickSFX,
            _ => defaultButtonClickSFX
        };

        if (clipToPlay == null)
        {
            Debug.LogError("No audio clip assigned!");
            return;
        }

        Debug.Log($"Playing clip: {clipToPlay.name}");
        Debug.Log($"AudioSource enabled: {audioSource.enabled}");
        Debug.Log($"AudioSource volume: {audioSource.volume}");

        float volumeToUse = volume > 0 ? volume : defaultVolume;
        audioSource.PlayOneShot(clipToPlay, volumeToUse);

        Debug.Log("PlayOneShot called successfully");
    }

    // These methods can be called directly from Button OnClick events in the inspector
    public void PlayDefaultSFX()
    {
        Debug.Log("PlayDefaultSFX called from button");
        PlayButtonSFX(ButtonSFXType.Default);
    }

    public void PlayConfirmSFX()
    {
        Debug.Log("PlayConfirmSFX called from button");
        PlayButtonSFX(ButtonSFXType.Confirm);
    }

    public void PlayCancelSFX()
    {
        Debug.Log("PlayCancelSFX called from button");
        PlayButtonSFX(ButtonSFXType.Cancel);
    }
}

public enum ButtonSFXType
{
    Default,
    Confirm,
    Cancel
}