using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string path => Application.persistentDataPath + "/MatchData.json";

    public static void Save(MatchData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static MatchData Load()
    {
        if (!File.Exists(path))
            return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<MatchData>(json);
    }

    public static void Clear()
    {
        if (File.Exists(path))
            File.Delete(path);
    }
}
[System.Serializable]
public class MatchData
{
    public int rows;
    public int columns;
    public int score;
    public int matchedPairs;
    public int moves;

    public List<int> cardIds;
    public List<bool> matchedState;
    public List<bool> flippedState;
}