using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Level3Logic : MonoBehaviour
{
    public static Level3Logic Instance;

    [Header("Result UI")]
    public ResultUIAnimator resultUI;

    [Header("Explosion UI")]
    public TMP_Text explosionText;

    [Header("Score Settings")]
    public int maxScore = 10000;
    public int winThreshold = 10;

    private int explosionCount = 0;
    private bool ended = false;

    void Awake()
    {
        Instance = this;
        UpdateExplosionText();
    }

    void Update()
    {
        if (ended) return;
        if (MusicManager.Instance == null) return;

        var audio = MusicManager.Instance.AudioSource;
        if (audio.clip != null && !audio.isPlaying)
        {
            EndLevel();
        }
    }

    public void RegisterExplosion()
    {
        explosionCount++;
        UpdateExplosionText();
    }

    void UpdateExplosionText()
    {
        if (explosionText != null)
            explosionText.text = $"Explosions: {explosionCount}";
    }

    private void EndLevel()
    {
        if (ended) return;
        ended = true;
        int obstacleCount = GameObject.FindGameObjectsWithTag("Obstacle").Length;
        bool win = obstacleCount < winThreshold;
        int finalScore = explosionCount * 100;
        finalScore = Mathf.Clamp(finalScore, 0, maxScore);
        SaveHighScore(finalScore);

        if (resultUI != null)
            resultUI.Show(win, finalScore);

        StopLevelMusic();
        StartCoroutine(ReturnToMenuAfterDelay());
    }


    private void SaveHighScore(int score)
    {
        int levelIndex = PlayerPrefs.GetInt("CURRENT_LEVEL_INDEX", -1);
        string key = $"LEVEL_{levelIndex}_HIGHSCORE";

        int oldScore = PlayerPrefs.GetInt(key, 0);
        if (score > oldScore)
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
        }
    }

    private void StopLevelMusic()
    {
        if (MusicManager.Instance == null) return;
        var audio = MusicManager.Instance.AudioSource;
        audio.Stop();
        audio.clip = null;
    }

    private IEnumerator ReturnToMenuAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        string levelScene = PlayerPrefs.GetString("CURRENT_LEVEL");
        if (!string.IsNullOrEmpty(levelScene))
            SceneManager.UnloadSceneAsync(levelScene);

        SceneManager.LoadSceneAsync("BackgroundScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("LevelSelect", LoadSceneMode.Additive);

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayWelcomeMusic();
    }
}
