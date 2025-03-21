using System.Collections;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public ChartParser chartParser;  // Reference to ChartParser
    public GameObject notePrefab;   // Note prefab
    public Transform[] lanes;       // Assign lane positions in Inspector

    private void Start()
    {
        chartParser.ParseChart();  // Parse the chart data
        StartCoroutine(SpawnNotes());  // Start spawning notes
    }

    private IEnumerator SpawnNotes()
    {
        foreach (var note in chartParser.notes)
        {
            yield return new WaitForSeconds(note.time);  // Wait for the correct time
            SpawnNote(note.lane);  // Spawn note at the correct lane
        }
    }

    private void SpawnNote(int lane)
    {
        Transform laneTransform = lanes[lane];  // Get the lane position
        Instantiate(notePrefab, laneTransform.position, Quaternion.identity);  // Instantiate the note prefab
    }
}
