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
    private Transform container;
    private Transform template;
    private List<HighscoreEntry> entryList;
    private List<Transform> transformList;
    private void Awake()
    {
        container = transform.Find("Entries");
        template = container.Find("Template");
        template.gameObject.SetActive(false);
        string jsonString = PlayerPrefs.GetString("highscores");
        Highscores scores = JsonUtility.FromJson<Highscores>(jsonString);
        transformList = new List<Transform>();
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
    private void AddNewEntry(int score)
    {
        HighscoreEntry entry = new HighscoreEntry { score = score, date = DateTime.Now.ToBinary() };
        string jsonString = PlayerPrefs.GetString("highscores");
        Highscores scores = JsonUtility.FromJson<Highscores>(jsonString);
        scores.entryList.Add(entry);
        scores.entryList.Sort();
        if (scores.entryList.Count > 9) scores.entryList.RemoveAt(9);
        string json = JsonUtility.ToJson(scores);
        PlayerPrefs.SetString("highscores", json);
        PlayerPrefs.Save();
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
