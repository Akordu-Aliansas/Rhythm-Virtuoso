using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChartParser;

public class LaneSpawner : MonoBehaviour
{
    public NoteSpawner noteSpawner;  // Reference to NoteSpawner
    public int laneNumber;
    private List<NoteData> notes;
    private GameObject notePrefab;   // Note prefab
    private GameObject heldNotePrefab;   // Note prefab
    private Material setMaterial;    // Set material of note
    private Material specialMaterial;   // Set material of special notes
    private MoveSpeedControl movementSpeed;

    public void StartLaneSpawn()
    {
        notes = noteSpawner.laneNotes[laneNumber];
        notePrefab = noteSpawner.notePrefab;
        heldNotePrefab = noteSpawner.heldNotePrefab;
        setMaterial = noteSpawner.setMaterial[laneNumber];
        specialMaterial = noteSpawner.setMaterial[5];
        movementSpeed = noteSpawner.movementSpeed;
        StartCoroutine(SpawnNotes());  // Start spawning notes
    }

    private IEnumerator SpawnNotes()
    {
        foreach (var note in notes)
        {
            yield return new WaitForSeconds(note.time - Time.timeSinceLevelLoad);
            SpawnNote(note.holdTime);
        }
    }

    private void SpawnNote(float holdTime)
    {
        GameObject currentNote = (GameObject)Instantiate(notePrefab, transform.position, notePrefab.transform.rotation);
        currentNote.GetComponent<Note>().setMaterial = setMaterial;
        currentNote.GetComponent<Note>().specialMaterial = specialMaterial;
        if (holdTime > 0)
        {
            GameObject heldNote = (GameObject)Instantiate(heldNotePrefab, transform.position, heldNotePrefab.transform.rotation);
            heldNote.GetComponent<HeldNote>().note = currentNote;
            heldNote.GetComponent<Renderer>().material = setMaterial;
            heldNote.transform.localScale = new Vector3(heldNote.transform.localScale.x, heldNote.transform.localScale.y, holdTime * movementSpeed.moveSpeed);
            heldNote.GetComponent<HeldNote>().setMaterial = setMaterial;
            heldNote.GetComponent<HeldNote>().specialMaterial = specialMaterial;
        }
        // Instantiate the note prefab
    }
}
