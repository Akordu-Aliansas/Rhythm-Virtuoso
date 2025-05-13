using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static ChartParser;

public class NoteSpawner : MonoBehaviour
{
    public AudioManager audioManager;
    public ChartParser chartParser;  // Reference to ChartParser
    public GameObject notePrefab;   // Note prefab
    public GameObject heldNotePrefab;
    public MoveSpeedControl movementSpeed;
    public LaneSpawner[] lanes;       // Assign lane positions in Inspector
    public Material[] setMaterial;    // Set material of note
    public List<NoteData>[] laneNotes;
    public bool isSpecial; // Are current notes special
    public bool spawnSpecial;
    public void StartSong()
    {
        isSpecial = false;
        laneNotes = new List<NoteData>[5];
        for (int i = 0; i < laneNotes.Length; i++)
        {
            laneNotes[i] = new List<NoteData>();
        }
        chartParser.ParseChart();  // Parse the chart data
        FilterNotesByLane();
        for (int i = 0; i < laneNotes.Length; i++)
        {
            lanes[i].StartLaneSpawn();
        }
        audioManager.PlaySong();
        StartCoroutine(SetStarPower());
    }
    private IEnumerator SetStarPower()
    {
        foreach (var special in chartParser.specials)
        {
            yield return new WaitForSeconds(special.startTime - Time.timeSinceLevelLoad);
            spawnSpecial = true;
            isSpecial = true;
            yield return new WaitForSeconds(special.duration);
            spawnSpecial = false;
            yield return new WaitForSeconds(FindAnyObjectByType<ChartParser>().delayToHitzone);
            if(isSpecial == true)
            {
                FindAnyObjectByType<StarPower>().IncrementStarPower();
                //Increment overdrive
                isSpecial = false;
            } 
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
