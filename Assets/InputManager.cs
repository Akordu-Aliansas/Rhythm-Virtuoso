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
        GameObject[] notes = GameObject.FindGameObjectsWithTag("Note");

        foreach (GameObject noteObj in notes)
        {
            Note noteScript = noteObj.GetComponent<Note>();

            // Skip if the note has been destroyed (null)
            if (noteScript == null)
                continue;

            if (noteScript.CanBeHit())
            {
                Debug.Log("Hit!");
                scoreManager.AddScore(100);  // Add score for hitting the note
                noteScript.DestroyNote();
                return; // Ensures only one note is hit per press
            }
        }

        Debug.Log("Miss!");
    }
}
