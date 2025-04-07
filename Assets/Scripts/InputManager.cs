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
    }

    void CheckForHit()
    {
        bool hitRegistered = false;
        foreach (Note note in hitZone.GetActiveNotes())
        {
            if (note == null || !note.CanBeHit()) continue;

            Debug.Log("Hit!");
            scoreManager.AddScore(100);
            note.DestroyNote();
            hitRegistered = true;
        }

        if (!hitRegistered)
            Debug.Log("Miss!");
    }
}
