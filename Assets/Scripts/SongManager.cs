using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [Header("Song elements")]
    public int id = 0;
    public List<int> bpms;
    public List<int> resolutions;
    public List<AudioClip> audioTracks;
    public List<TextAsset> charts;
    [Header("Variables to change")]
    public TickRate tickRate;
    public AudioManager audioManager;
    public ChartParser parser;
    [Header("Note Spawner to start")]
    public NoteSpawner spawner;
    public class Song
    {
        public int BPM;
        public int resolution;
        public AudioClip audio;
        public TextAsset chart;
    }
    public void Start()
    {
        if (FindAnyObjectByType<ChangeSelectedSong>() != null) id = FindAnyObjectByType<ChangeSelectedSong>().selectedSong;
        Song current = new Song { BPM = bpms[id], resolution = resolutions[id], audio = audioTracks[id], chart = charts[id] };
        tickRate.bpm = current.BPM;
        tickRate.resolution = current.resolution;
        audioManager.songClip = current.audio;
        parser.chartFile = current.chart;
        tickRate.ready = true;
        spawner.StartSong();
    }
}
