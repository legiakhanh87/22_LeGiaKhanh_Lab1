using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float speed = 3f;
    private Vector2 direction;

    void Start()
    {
        // Random hướng di chuyển ban đầu
        direction = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Chạm tường → bounce
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector2 normal = collision.contacts[0].normal;
            direction = Vector2.Reflect(direction, normal);
        }

        // Chạm object khác → cả 2 biến mất
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);

            // 👉 Level 3 sẽ tăng biến đếm + explosion ở đây
        }
    }
}
