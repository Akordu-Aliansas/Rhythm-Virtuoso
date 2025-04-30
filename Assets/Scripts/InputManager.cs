using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public Key keyToPress;
    public HitZone hitZone;
    public ScoreManager scoreManager;
    public string playerPrefKey;

    [Header("Feedback")]
    public AudioClip hitSound;
    public AudioClip missSound;
    public ParticleSystem perfectHitParticles; // Assign in Inspector
    public ParticleSystem goodHitParticles;
    public ParticleSystem okHitParticles;

    void Awake()
    {
        if (PlayerPrefs.HasKey(playerPrefKey))
        {
            string savedKey = PlayerPrefs.GetString(playerPrefKey);
            if (System.Enum.TryParse<Key>(savedKey, out Key parsed))
            {
                keyToPress = parsed;
            }
        }
    }

    void Update()
    {
        if (Keyboard.current[keyToPress].wasPressedThisFrame)
        {
            CheckForHit();
        }
        else if (Keyboard.current[keyToPress].isPressed)
        {
            CheckForHold();
        }
        else if (Keyboard.current[keyToPress].wasReleasedThisFrame)
        {
            CheckForRelease();
        }
    }

    void CheckForHit()
    {
        bool hitRegistered = false;

        foreach (Note note in hitZone.GetActiveNotes())
        {
            if (note == null || !note.CanBeHit()) continue;

            // Calculate hit accuracy
            float hitAccuracy = Mathf.Abs(note.transform.position.z);
            HitAccuracy accuracy = GetHitAccuracy(hitAccuracy);

            // Play appropriate effects
            PlayHitEffects(accuracy, note.transform.position);

            // Score and destroy
            scoreManager.AddScore(GetScoreForAccuracy(accuracy));
            note.DestroyNote();
            hitRegistered = true;

            // Handle held notes
            HeldNote heldNote = hitZone.GetHeldNote();
            if (heldNote != null && !heldNote.wasReleased)
                heldNote.isHeld = true;
        }

        if (!hitRegistered)
        {
            Debug.Log("Miss!");
        }
    }

    HitAccuracy GetHitAccuracy(float zPosition)
    {
        if (zPosition < 0.1f) return HitAccuracy.Perfect;
        if (zPosition < 0.3f) return HitAccuracy.Good;
        return HitAccuracy.Okay;
    }

    int GetScoreForAccuracy(HitAccuracy accuracy)
    {
        switch (accuracy)
        {
            case HitAccuracy.Perfect: return 100;
            case HitAccuracy.Good: return 75;
            default: return 50;
        }
    }

    void PlayHitEffects(HitAccuracy accuracy, Vector3 position)
    {
        ParticleSystem particlesToPlay = null;

        switch (accuracy)
        {
            case HitAccuracy.Perfect:
                particlesToPlay = perfectHitParticles;
                break;
            case HitAccuracy.Good:
                particlesToPlay = goodHitParticles;
                break;
            default:
                particlesToPlay = okHitParticles;
                break;
        }

        if (particlesToPlay != null)
        {
            Instantiate(particlesToPlay, position, Quaternion.identity)
                .Play();
        }
    }

    void CheckForHold()
    {
        HeldNote note = hitZone.GetHeldNote();
        if (note != null && note.isHeld)
        {
            scoreManager.AddScore(10 * Time.deltaTime);
        }
    }

    void CheckForRelease()
    {
        HeldNote note = hitZone.GetHeldNote();
        if (note != null && note.isHeld)
        {
            note.Release();
        }
    }
}

public enum HitAccuracy
{
    Perfect,
    Good,
    Okay
}