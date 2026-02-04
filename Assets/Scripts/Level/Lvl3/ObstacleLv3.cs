using UnityEngine;

public class Obstacle3 : MonoBehaviour
{
    public float speed = 3f;
    private Vector2 direction;

    [Header("Explosion / Bounce Effect")]
    public GameObject bounceEffectPrefab;
    public float bounceEffectScaleMultiplier = 0.2f;
    public float minEffectScale = 0.3f;
    public float maxEffectScale = 1.2f;

    private bool exploded = false;

    void Start()
    {
        direction = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 🚫 nếu đã nổ rồi thì thôi
        if (exploded) return;

        // 🧱 CHẠM TƯỜNG → BOUNCE
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 normal = collision.contacts[0].normal;
            direction = Vector2.Reflect(direction, normal);
        }

        // 💥 CHẠM OBSTACLE → NỔ
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            exploded = true;

            SpawnEffect(collision);

            // 🔢 báo manager tăng số vụ nổ
            if (Level3Logic.Instance != null)
            {
                Level3Logic.Instance.RegisterExplosion();
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void SpawnEffect(Collision2D collision)
    {
        if (bounceEffectPrefab == null) return;

        Vector2 contactPoint = collision.GetContact(0).point;

        GameObject effect = Instantiate(
            bounceEffectPrefab,
            contactPoint,
            Quaternion.identity
        );

        float impact = collision.relativeVelocity.magnitude;
        float scale = Mathf.Clamp(
            impact * bounceEffectScaleMultiplier,
            minEffectScale,
            maxEffectScale
        );

        effect.transform.localScale = Vector3.one * scale;

        Destroy(effect, 1f);
    }
}
