using UnityEngine;

public static class HighScoreManager
{
    public static string GetKey(int levelIndex)
    {
        return $"LEVEL_{levelIndex}_HIGHSCORE";
    }

    public static int GetHighScore(int levelIndex)
    {
        return PlayerPrefs.GetInt(GetKey(levelIndex), 0);
    }

    public static void SaveHighScore(int levelIndex, int score)
    {
        int current = GetHighScore(levelIndex);
        if (score > current)
        {
            PlayerPrefs.SetInt(GetKey(levelIndex), score);
            PlayerPrefs.Save();
        }
    }
}
