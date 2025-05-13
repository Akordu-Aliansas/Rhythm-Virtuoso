using System;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreUpdate : MonoBehaviour
{
    public int songID;
    public void AddNewEntry(int score)
    {
        songID = FindAnyObjectByType<SongManager>().id;
        HighscoreEntry entry = new HighscoreEntry { score = score, date = DateTime.Now.ToBinary() };
        string jsonString = PlayerPrefs.GetString("highscores"+songID);
        Highscores scores = JsonUtility.FromJson<Highscores>(jsonString);
        if (scores == null) scores = new Highscores { entryList = new List<HighscoreEntry>() };
        scores.entryList.Add(entry);
        scores.entryList.Sort();
        if (scores.entryList.Count > 9) scores.entryList.RemoveAt(9);
        string json = JsonUtility.ToJson(scores);
        PlayerPrefs.SetString("highscores"+songID, json);
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
