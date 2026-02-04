using UnityEngine;

public class PulseWithMusic : MonoBehaviour
{
    public float multiplier = 5f;
    public float smoothSpeed = 10f;

    Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        if (MusicManager.Instance == null) return;

        float[] spectrum = MusicManager.Instance.spectrum;

        float sum = 0f;
        for (int i = 0; i < 40; i++)
        {
            sum += spectrum[i];
        }

        float intensity = (sum / 40f) * multiplier;

        Vector3 targetScale = baseScale * (1f + intensity);
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * smoothSpeed
        );
    }
}
