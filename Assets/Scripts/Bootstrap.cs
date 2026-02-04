using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        // Load background dùng chung
        SceneManager.LoadScene("BackgroundScene", LoadSceneMode.Additive);

        // Load màn Welcome
        SceneManager.LoadScene("WelcomeScene", LoadSceneMode.Additive);
    }
}
