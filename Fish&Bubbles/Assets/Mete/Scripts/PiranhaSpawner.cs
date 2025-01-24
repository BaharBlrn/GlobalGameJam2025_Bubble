using UnityEngine;

public class PiranhaSpawner : MonoBehaviour
{
    [Header("Piranha Settings")]
    public GameObject piranhaPrefab; // Assign your Piranha prefab in the inspector
    public float spawnInterval = 5f; // Time between Piranha spawns
    public float followDuration = 10f; // Time the Piranha will chase the player
    public float speed = 5f; // Piranha movement speed
    public float spawnDistance = 3f; // Distance offscreen where the Piranha spawns

    private float screenTop;
    private float screenBottom;
    private float screenLeft;
    private float screenRight;

    void Start()
    {
        // Calculate screen boundaries in world coordinates
        screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        InvokeRepeating(nameof(SpawnPiranha), 0f, spawnInterval);
    }

    void SpawnPiranha()
    {
        // Randomly spawn the Piranha outside the screen bounds
        float spawnX = Random.value > 0.5f ? screenLeft - spawnDistance : screenRight + spawnDistance;
        float spawnY = Random.Range(screenBottom - spawnDistance, screenTop + spawnDistance);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        // Instantiate the Piranha
        GameObject piranha = Instantiate(piranhaPrefab, spawnPosition, Quaternion.identity);

        // Start the Piranha behavior
        PiranhaBehavior behavior = piranha.AddComponent<PiranhaBehavior>();
        behavior.Init(followDuration, speed);

        // Destroy the Piranha after the follow duration
        Destroy(piranha, followDuration);
    }
}
