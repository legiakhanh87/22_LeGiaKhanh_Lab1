using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneButtonUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Scene")]
    [SerializeField] private string targetScene;   
    [SerializeField] private string unloadScene;  
    public void OnPointerClick(PointerEventData eventData)
    { 
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetScene)
        {
            SceneManager.SetActiveScene(scene);

            if (!string.IsNullOrEmpty(unloadScene))
                SceneManager.UnloadSceneAsync(unloadScene);

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
