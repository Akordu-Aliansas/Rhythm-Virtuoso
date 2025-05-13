using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    public int songID;
    private Transform container;
    private Transform template;
    private List<HighscoreEntry> entryList;
    private List<Transform> transformList;
    private void Awake()
    {
        songID = FindAnyObjectByType<ChangeSelectedSong>().selectedSong;
        container = transform.Find("Entries");
        template = container.Find("Template");
        template.gameObject.SetActive(false);
        transformList = new List<Transform>();
        string jsonString = PlayerPrefs.GetString("highscores" + songID);
        Highscores scores = JsonUtility.FromJson<Highscores>(jsonString);
        if (scores == null) scores = new Highscores { entryList = new List<HighscoreEntry>() };
        foreach (HighscoreEntry entry in scores.entryList) AddEntry(entry, container, transformList);
    }
    public void ChangePage(bool next)
    {
        foreach (Transform transform in transformList) Destroy(transform.gameObject);
        if (next && songID < 5) songID++;
        else if (!next && songID > 0) songID--;
        transformList = new List<Transform>();
        string jsonString = PlayerPrefs.GetString("highscores" + songID);
        Highscores scores = JsonUtility.FromJson<Highscores>(jsonString);
        if (scores == null) scores = new Highscores { entryList = new List<HighscoreEntry>() };
        foreach (HighscoreEntry entry in scores.entryList) AddEntry(entry, container, transformList);
    }
    private void AddEntry(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 60f;
        Transform entry = Instantiate(template, container);
        RectTransform entryRect = entry.GetComponent<RectTransform>();
        entryRect.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entry.gameObject.SetActive(true);
        entry.Find("Row number").GetComponent<TMP_Text>().text = (transformList.Count + 1).ToString();
        entry.Find("Score").GetComponent<TMP_Text>().text = highscoreEntry.score.ToString();
        entry.Find("Date").GetComponent<TMP_Text>().text = DateTime.FromBinary(highscoreEntry.date).ToString("d");
        transformList.Add(entry);
    }

    private class Highscores { public List<HighscoreEntry> entryList; }
    [Serializable]
    private class HighscoreEntry : IComparable<HighscoreEntry>
    {
        public int score;
        public long date;
        public int CompareTo(HighscoreEntry other)
        {
            if (score < other.score) return 1;
            return -1;
        }
    }
}
