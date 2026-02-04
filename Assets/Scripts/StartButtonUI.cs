using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartButtonUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private LevelListUI levelList;

    public void OnPointerClick(PointerEventData eventData)
    {
        int index = levelList.GetSelectedLevelIndex();
        if (index < 0) return;

        string levelScene = "Level" + index;

        // 🔥 FIX Ở ĐÂY
        PlayerPrefs.SetInt("CURRENT_LEVEL_INDEX", index);
        PlayerPrefs.SetString("CURRENT_LEVEL", levelScene);
        PlayerPrefs.Save();

        SafeUnload("WelcomeScene");
        SafeUnload("LevelSelect");
        SafeUnload("BackgroundScene");

        SceneManager.LoadSceneAsync(levelScene, LoadSceneMode.Additive);
    }


    void SafeUnload(string sceneName)
    {
        var s = SceneManager.GetSceneByName(sceneName);
        if (s.isLoaded)
            SceneManager.UnloadSceneAsync(sceneName);
    }
}
