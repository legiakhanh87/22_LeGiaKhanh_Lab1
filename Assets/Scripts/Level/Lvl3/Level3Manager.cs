using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3Manager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public static int InitialObstacleCount;
    public Collider2D Border_Top;
    public Collider2D Border_Right;
    public Collider2D Border_Left;
    public Collider2D Border_Bottom;

    public int spawnCount = 20;
    public float padding = 0.5f;

    void Start()
    {
        SpawnObjects();
        InitialObstacleCount = spawnCount;
    }

    void SpawnObjects()
    {
        Scene levelScene = SceneManager.GetSceneByName("Level3");
        float obstacleRadius = obstaclePrefab
            .GetComponent<Collider2D>()
            .bounds.extents.x;

        float minX = Border_Left.bounds.max.x + padding + obstacleRadius;
        float maxX = Border_Right.bounds.min.x - padding - obstacleRadius;
        float minY = Border_Bottom.bounds.max.y + padding + obstacleRadius;
        float maxY = Border_Top.bounds.min.y - padding - obstacleRadius;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 spawnPos = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            GameObject obj = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(obj, levelScene);
        }
    }
}
