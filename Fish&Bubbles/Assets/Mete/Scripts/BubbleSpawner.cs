using UnityEngine;
using System.Collections;

public class BubbleSpawner : MonoBehaviour
{
    [Header("Bubble Settings")]
    public GameObject bubblePrefab; // Assign your bubble prefab in the inspector
    public int numberOfBubbles = 10; // Number of bubbles to spawn
    public float spawnInterval = 0.5f; // Time between spawns
    public float minSpeed = 0.5f; // Minimum bubble speed
    public float maxSpeed = 1f; // Maximum bubble speed
    public float swayAmount = 0.5f; // Amount of left-right sway
    public float swaySpeed = 4.0f; // Speed of sway

    private float screenBottom;
    private float screenLeft;
    private float screenRight;

    void Start()
    {
        // Calculate screen boundaries in world coordinates
        screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        StartCoroutine(SpawnBubbles());
    }

    IEnumerator SpawnBubbles()
    {
        for (int i = 0; i < numberOfBubbles; i++)
        {
            SpawnBubble();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnBubble()
    {
        float randomX = Random.Range(screenLeft, screenRight); // Random X position within screen width
        Vector3 spawnPosition = new Vector3(randomX, screenBottom, 0);
        GameObject newBubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

        // Scale bubble size based on screen resolution
        float scaleFactor = Screen.height / 1920f +0.4f; // Assuming 1920 is the baseline resolution
        newBubble.transform.localScale *= scaleFactor;

        // Assign upward movement speed
        float speed = Random.Range(minSpeed, maxSpeed);
        Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, speed);

        // Add a sway movement
        StartCoroutine(SwayBubble(newBubble, rb));

        // Destroy bubble when it goes offscreen
        Destroy(newBubble, 10f); // Ensure cleanup if something goes wrong
    }

    IEnumerator SwayBubble(GameObject bubble, Rigidbody2D rb)
    {
        while (bubble != null)
        {
            float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            if (rb != null)
            {
                rb.velocity = new Vector2(sway, rb.velocity.y);
            }
            yield return null;
        }
    }

    void OnBecameInvisible()
    {
        // Destroy the bubble if it goes offscreen
        Destroy(gameObject);
    }
}
