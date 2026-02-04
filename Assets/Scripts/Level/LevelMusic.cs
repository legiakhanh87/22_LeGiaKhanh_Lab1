using UnityEngine;
using UnityEngine.Rendering;

public class LevelMusic : MonoBehaviour
{
    public AudioClip levelMusic;

    void Start()
    {
        var music = MusicManager.Instance;
        music.AudioSource.loop = false; 
        music.PlayLevelMusic(levelMusic);
    }
}
