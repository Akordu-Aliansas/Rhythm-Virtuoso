using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public Key keyToPress;  // Use "Key" from the new Input System
    public HitZone hitZone; // Reference to HitZone
    public ScoreManager scoreManager;

    void Update()
    {
        if (Keyboard.current[keyToPress].wasPressedThisFrame) // New Input System method
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

            Debug.Log("Hit!");
            AudioManager.Instance.PlayHitSound();
            scoreManager.AddScore(100);
            HeldNote heldNote = hitZone.GetHeldNote();
            if (heldNote != null && !heldNote.wasReleased) heldNote.isHeld = true;
            note.DestroyNote();
            hitRegistered = true;
        }

        if (!hitRegistered)
            Debug.Log("Miss!");
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
