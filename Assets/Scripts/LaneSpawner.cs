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
    private Material setMaterial;    // Set material of note

    public void StartLaneSpawn()
    {
        notes = noteSpawner.laneNotes[laneNumber];
        notePrefab = noteSpawner.notePrefab;
        setMaterial = noteSpawner.setMaterial[laneNumber];
        StartCoroutine(SpawnNotes());  // Start spawning notes
    }

    private IEnumerator SpawnNotes()
    {
        float audioStartTime = AudioManager.Instance.GetSongTime();

        foreach (var note in notes)
        {
            float waitDuration = note.time - (AudioManager.Instance.GetSongTime() - audioStartTime);
            if (waitDuration > 0)
                yield return new WaitForSeconds(waitDuration);

            SpawnNote();
        }
    }

    private void SpawnNote()
    {
        GameObject currentNote = (GameObject)Instantiate(notePrefab, transform.position, notePrefab.transform.rotation);
        currentNote.GetComponent<Renderer>().material = setMaterial;
        // Instantiate the note prefab
    }
}
