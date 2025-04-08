using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChartParser;

public class NoteSpawner : MonoBehaviour
{
    public ChartParser chartParser;  // Reference to ChartParser
    public GameObject notePrefab;   // Note prefab
    public LaneSpawner[] lanes;       // Assign lane positions in Inspector
    public Material[] setMaterial;    // Set material of note
    public List<NoteData>[] laneNotes;

    private void Start()
    {
        AudioManager.Instance.PlaySong();
        laneNotes = new List<NoteData>[5];
        for(int i = 0; i < laneNotes.Length; i++)
        {
            laneNotes[i] = new List<NoteData>();
        }
        chartParser.ParseChart();  // Parse the chart data
        FilterNotesByLane();
        for (int i = 0; i < laneNotes.Length; i++)
        {
            lanes[i].StartLaneSpawn();
        }
    }

    private void FilterNotesByLane()
    {
        foreach(var note in chartParser.notes)
        {
            laneNotes[note.lane].Add(note);
        }
    }
}
