using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Level2Logic : MonoBehaviour
{
    [Header("Result UI")]
    public ResultUIAnimator resultUI;

    [Header("Score Settings")]
    public int winThreshold = 10; 
    private bool ended = false;
    private int initialObstacle;
    void Start()
    {
        initialObstacle = Level2Manager.InitialObstacleCount;
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

    private void EndLevel()
    {
        if (ended) return;
        ended = true;
        int obstacleCount = GameObject.FindGameObjectsWithTag("Obstacle").Length;
        bool win = (initialObstacle-obstacleCount) < winThreshold;
        int finalScore=obstacleCount*10;       
        SaveHighScore(finalScore);
        if (resultUI != null)
        {
            resultUI.Show(win, finalScore);
        }
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
        {
            var s = SceneManager.GetSceneByName(levelScene);
            if (s.isLoaded)
                SceneManager.UnloadSceneAsync(levelScene);
        }

        SceneManager.LoadSceneAsync("BackgroundScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("LevelSelect", LoadSceneMode.Additive);

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayWelcomeMusic();
    }
    public void BackToLevelSelect()
    {
        if (ended) return;

        ended = true;
        StopAllCoroutines();

        StopLevelMusic();

        string levelScene = PlayerPrefs.GetString("CURRENT_LEVEL");

        if (!string.IsNullOrEmpty(levelScene))
        {
            var s = SceneManager.GetSceneByName(levelScene);
            if (s.isLoaded)
                SceneManager.UnloadSceneAsync(levelScene);
        }

        SceneManager.LoadSceneAsync("BackgroundScene", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("LevelSelect", LoadSceneMode.Additive);

        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayWelcomeMusic();
    }
}
