using UnityEngine;

public class PlayerEffectsManager : MonoBehaviour
{
    [Header("Trail Settings")]
    [Tooltip("Index của trail (theo thứ tự child)")]
    [SerializeField] int trailIndex = 0;

    public static string CurrentEffectName { get; private set; }

    ParticleSystem[] m_effectParticleSystems;

    void Awake()
    {
        LoadTrailFromInspector();
    }

    void LoadTrailFromInspector()
    {
        if (transform.childCount == 0)
        {
            Debug.LogWarning("No trail child found");
            return;
        }

        // Clamp để tránh crash
        trailIndex = Mathf.Clamp(trailIndex, 0, transform.childCount - 1);

        // Disable tất cả trail
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        // Enable trail được chọn
        Transform trail = transform.GetChild(trailIndex);
        trail.gameObject.SetActive(true);

        CurrentEffectName = trail.name;
        m_effectParticleSystems =
            trail.GetComponentsInChildren<ParticleSystem>(true);
    }

    // Optional: dùng cho PlayerController
    public void Play()
    {
        if (m_effectParticleSystems == null) return;

        foreach (var ps in m_effectParticleSystems)
            ps.Play();
    }

    public void Stop()
    {
        if (m_effectParticleSystems == null) return;

        foreach (var ps in m_effectParticleSystems)
            ps.Stop();
    }
}
