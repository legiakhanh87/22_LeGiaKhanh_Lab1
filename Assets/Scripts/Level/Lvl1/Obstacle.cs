using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    public GameObject bounceEffectPrefab;

    public float minSize = 6f;
    public float maxSize = 10f;

    public float minSpeed = 50f;
    public float maxSpeed = 100f;

    public float maxSpinSpeed = 10f;

    [Header("Bounce Effect")]
    public float bounceEffectScaleMultiplier = 0.15f;
    public float minEffectScale = 0.2f;
    public float maxEffectScale = 1.0f;

    private Rigidbody2D _rb;
    private float targetSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Random size
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        // Speed phụ thuộc size
        targetSpeed = Random.Range(minSpeed, maxSpeed) / randomSize;

        // Random direction
        Vector2 dir = Random.insideUnitCircle.normalized;
        _rb.linearVelocity = dir * targetSpeed;

        // Spin
        float randomSpinSpeed = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        _rb.angularVelocity = randomSpinSpeed;
    }

    private void FixedUpdate()
    {
        // Giữ nguyên tốc độ sau va chạm
        if (_rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            _rb.linearVelocity =
                _rb.linearVelocity.normalized * targetSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (bounceEffectPrefab == null) return;

        var contactPoint = other.GetContact(0).point;
        var bounceEffect = Instantiate(
            bounceEffectPrefab,
            contactPoint,
            Quaternion.identity
        );

        float impact = other.relativeVelocity.magnitude;
        float scale = Mathf.Clamp(
            impact * bounceEffectScaleMultiplier,
            minEffectScale,
            maxEffectScale
        );

        bounceEffect.transform.localScale = Vector3.one * scale;

        Destroy(bounceEffect, 1f);
    }
}
