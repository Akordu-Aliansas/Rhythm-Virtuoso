using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChartParser : MonoBehaviour
{
    public TextAsset chartFile; // Assign in Inspector or load dynamically
    public List<NoteData> notes = new List<NoteData>();
    public List<StarpowerData> specials = new List<StarpowerData>();
    public TickRate tickRate;
    public MoveSpeedControl movementSpeed;
    public float delayToHitzone;

    [System.Serializable]
    public class NoteData
    {
        public float time; // Time in seconds
        public int lane;   // Lane (0-4)
        public float holdTime; // Hold time in seconds
    }
    [System.Serializable]
    public class StarpowerData
    {
        public float startTime; // Start time
        public float duration; //  Duration in seconds
    }
    public void ParseChart()
    {
        if (chartFile == null)
        {
            Debug.LogError("Chart file is missing!");
            return;
        }
        delayToHitzone = (4.5f / movementSpeed.moveSpeed);
        tickRate.waitTime = Mathf.Max(delayToHitzone, tickRate.waitTime);
        if (delayToHitzone > tickRate.waitTime) tickRate.waitTime = delayToHitzone;
        string[] lines = chartFile.text.Split('\n');
        bool inNotesSection = false;
        foreach (string line in lines)
        {
            string trimmed = line.Trim();

            // Start of the notes section
            if (trimmed.StartsWith("[ExpertSingle]"))
            {
                inNotesSection = true;
                continue;
            }

            // End of the notes section
            if (trimmed.StartsWith("}"))
            {
                inNotesSection = false;
                continue;
            }

            // Parse note data
            if (inNotesSection && trimmed.Contains("N"))
            {
                string[] parts = trimmed.Split(' ');

                // Ensure we have at least 3 parts (tick, N, lane)
                if (parts.Length >= 3)
                {
                    int tick = int.Parse(parts[0]);  // Tick position
                    int lane = int.Parse(parts[3]);  // Lane (0-4)
                    int holdTick = int.Parse(parts[4]); // Hold time
                    if (lane < 5)
                    {
                        float time = TickToSeconds(tick, tickRate.bpm, tickRate.resolution);
                        float holdTime = TickToSeconds(holdTick, tickRate.bpm, tickRate.resolution);
                        notes.Add(new NoteData { time = time + tickRate.waitTime - delayToHitzone, lane = lane, holdTime = holdTime });
                    }
                }
            }
            if (inNotesSection && trimmed.Contains("S"))
            {
                string[] parts = trimmed.Split(' ');

                // Ensure we have at least 3 parts (tick, S, lane)
                if (parts.Length >= 3)
                {
                    int tick = int.Parse(parts[0]);  // Tick position
                    int lane = int.Parse(parts[3]);  // Lane (0-4)
                    int durationTicks = int.Parse(parts[4]); // Hold time
                    if (lane == 2)
                    {
                        float start = TickToSeconds(tick, tickRate.bpm, tickRate.resolution);
                        float duration = TickToSeconds(durationTicks, tickRate.bpm, tickRate.resolution);
                        specials.Add(new StarpowerData { startTime = start + tickRate.waitTime - delayToHitzone, duration = duration });
                    }
                }
            }
        }

        Debug.Log($"Parsed {notes.Count} notes!");
    }

    private float TickToSeconds(int tick, float bpm, int resolution)
    {
        float beatDuration = 60f / bpm;
        return (tick / (float)resolution) * beatDuration;
    }
}
