using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Default / Welcome Music")]
    [SerializeField] private AudioClip welcomeMusic;
    [SerializeField] private float welcomeVolume = 1f;

    public AudioSource AudioSource { get; private set; }

    // Spectrum data cho visual / pulse
    public float[] spectrum = new float[256];

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        AudioSource = GetComponent<AudioSource>();
        AudioSource.loop = true;
        AudioSource.playOnAwake = false;
    }

    void Start()
    {
        if (welcomeMusic != null)
        {
            PlayMusic(welcomeMusic, welcomeVolume);
        }
    }

    void Update()
    {
        if (AudioSource != null && AudioSource.isPlaying)
        {
            AudioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
        }
    }

    /// <summary>
    /// Dùng cho menu / welcome
    /// </summary>
    public void PlayWelcomeMusic()
    {
        if (welcomeMusic == null) return;
        PlayMusic(welcomeMusic, welcomeVolume);
    }

    /// <summary>
    /// Dùng cho level
    /// </summary>
    public void PlayLevelMusic(AudioClip clip, float volume = 1f)
    {
        PlayMusic(clip, volume);
    }

    /// <summary>
    /// Core play logic (KHÔNG restart nếu đang phát đúng clip)
    /// </summary>
    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        if (AudioSource.clip == clip && AudioSource.isPlaying)
            return;

        AudioSource.Stop();
        AudioSource.clip = clip;
        AudioSource.volume = volume;
        AudioSource.Play();
    }

    public void StopMusic()
    {
        AudioSource.Stop();
    }
}
 