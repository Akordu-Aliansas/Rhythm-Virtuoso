using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class SongEntry
{
    public string name;
    public AudioClip clip;
    public Texture2D background;
}

public class ContentLoader : MonoBehaviour
{
    // Where on disk we look for user-added files:
    private string songsDir;
    private string bgsDir;

    // After loading, youÅfll have a list you can bind to your song-select UI
    public List<SongEntry> loadedSongs = new List<SongEntry>();

    IEnumerator Start()
    {
        // 1) Define the two ÅgmodÅh folders under persistentDataPath
        songsDir = Path.Combine(Application.persistentDataPath, "Songs");
        bgsDir = Path.Combine(Application.persistentDataPath, "Backgrounds");

        // 2) Create them if they donÅft exist
        Directory.CreateDirectory(songsDir);
        Directory.CreateDirectory(bgsDir);

        // 3) (Optional) Copy built-in defaults from StreamingAssets/Songs into persistentDataPath,
        //    so your shipped songs show up alongside user-added ones.
        //    You can skip this if you only care about user content.
        //    yield return StartCoroutine(CopyStreamingDefaults("Songs"));
        //    yield return StartCoroutine(CopyStreamingDefaults("Backgrounds"));

        // 4) Scan for audio files
        string[] audioFiles = Directory.GetFiles(songsDir, "*.*")
            .Where(f => f.EndsWith(".wav") || f.EndsWith(".mp3") || f.EndsWith(".ogg"))
            .ToArray();

        // 5) Scan for image files (backgrounds)
        string[] bgFiles = Directory.GetFiles(bgsDir, "*.*")
            .Where(f => f.EndsWith(".png") || f.EndsWith(".jpg") || f.EndsWith(".jpeg"))
            .ToArray();

        // 6) For each audio file, load the AudioClip and pick a matching bg (by filename)
        foreach (var audioPath in audioFiles)
        {
            string fileNameNoExt = Path.GetFileNameWithoutExtension(audioPath);
            string bgMatch = System.Array.Find(bgFiles, p =>
                Path.GetFileNameWithoutExtension(p).Equals(fileNameNoExt, System.StringComparison.OrdinalIgnoreCase));

            // Load audio
            var audioRequest = UnityWebRequestMultimedia.GetAudioClip("file://" + audioPath, AudioType.UNKNOWN);
            yield return audioRequest.SendWebRequest();
            if (audioRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"Failed loading audio at {audioPath}: {audioRequest.error}");
                continue;
            }
            var clip = DownloadHandlerAudioClip.GetContent(audioRequest);

            // Load background (if found)
            Texture2D bgTex = null;
            if (bgMatch != null)
            {
                var texRequest = UnityWebRequestTexture.GetTexture("file://" + bgMatch);
                yield return texRequest.SendWebRequest();
                if (texRequest.result == UnityWebRequest.Result.Success)
                    bgTex = DownloadHandlerTexture.GetContent(texRequest);
                else
                    Debug.LogWarning($"Failed loading bg at {bgMatch}: {texRequest.error}");
            }

            // Add into our list
            loadedSongs.Add(new SongEntry
            {
                name = fileNameNoExt,
                clip = clip,
                background = bgTex
            });
        }

        // 7) Hand the list over to your song-select UI
        // e.g. YourSongSelectUI.Instance.Populate(loadedSongs);
    }

    // Optional helper to copy built-ins out of StreamingAssets into persistentDataPath
    private IEnumerator CopyStreamingDefaults(string folderName)
    {
        string srcDir = Path.Combine(Application.streamingAssetsPath, folderName);
        string dstDir = Path.Combine(Application.persistentDataPath, folderName);

        if (!Directory.Exists(srcDir))
            yield break;

        foreach (var file in Directory.GetFiles(srcDir))
        {
            string fname = Path.GetFileName(file);
            string dst = Path.Combine(dstDir, fname);
            if (File.Exists(dst))
                continue; // donÅft overwrite userÅfs version

            var req = UnityWebRequest.Get(Path.Combine("file://", file));
            yield return req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
                File.WriteAllBytes(dst, req.downloadHandler.data);
        }
    }
}
