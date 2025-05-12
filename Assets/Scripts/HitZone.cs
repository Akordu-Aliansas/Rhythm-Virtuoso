using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    private List<Note> activeNotes = new List<Note>();
    private HeldNote heldNote = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            Note note = other.GetComponent<Note>();
            note.SetCanBeHit(true);
            activeNotes.Add(note);
        }
        if (other.CompareTag("HeldNote"))
        {
            HeldNote note = other.GetComponent<HeldNote>();
            note.SetCanBeHit(true);
            heldNote = note;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            Note note = other.GetComponent<Note>();
            if (note.isSpecial) {
                FindAnyObjectByType<NoteSpawner>().isSpecial = false;
                FindAnyObjectByType<NoteSpawner>().spawnSpecial = false;
            }
            note.SetCanBeHit(false);
            activeNotes.Remove(note);
            //Debug.Log("Miss");
        }
        if (other.CompareTag("HeldNote"))
        {
            HeldNote note = other.GetComponent<HeldNote>();
            note.SetCanBeHit(false);
            heldNote = null;
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
    public List<Note> GetActiveNotes() { return activeNotes; }
    public HeldNote GetHeldNote() { return heldNote; }
}
