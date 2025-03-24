using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    private List<Note> activeNotes = new List<Note>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            Note note = other.GetComponent<Note>();
            note.SetCanBeHit(true);
            activeNotes.Add(note);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            Note note = other.GetComponent<Note>();
            note.SetCanBeHit(false);
            activeNotes.Remove(note);
        }
    }

    public Note GetClosestNote()
    {
        if (activeNotes.Count == 0) return null;

        // Find the note closest to the hit position (Z = 0)
        Note closestNote = activeNotes[0];
        float minDistance = Mathf.Abs(closestNote.transform.position.z);

        foreach (Note note in activeNotes)
        {
            float distance = Mathf.Abs(note.transform.position.z);
            if (distance < minDistance)
            {
                closestNote = note;
                minDistance = distance;
            }
        }

        return closestNote;
    }
}
