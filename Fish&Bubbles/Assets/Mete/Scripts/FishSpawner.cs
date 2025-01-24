using UnityEngine;
using System.Collections;

public class FishSpawner : MonoBehaviour
{
    [Header("Fish Settings")]
    public GameObject fishPrefab; // Assign your fish prefab in the inspector
    public int numberOfFish = 10; // Number of fish to spawn
    public float spawnInterval = 2.0f; // Time between spawns
    public float minSpeed = 2f; // Minimum fish speed
    public float maxSpeed = 5f; // Maximum fish speed
    public float swayAmount = 0.5f; // Amplitude of the up and down sway
    public float swaySpeed = 2.0f; // Speed of the sway motion
    public float baseScreenHeight = 1920f; // Reference screen height for scaling

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

        StartCoroutine(SpawnFish());
    }

    IEnumerator SpawnFish()
    {
        for (int i = 0; i < numberOfFish; i++)
        {
            SpawnSingleFish();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnSingleFish()
    {
        // Randomly decide if the fish spawns from the left or right side
        bool spawnFromLeft = Random.value > 0.5f;

        // Set the spawn position
        float spawnX = spawnFromLeft ? screenLeft : screenRight;
        float spawnY = Random.Range(screenBottom, screenTop); // Random Y position within screen height
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        // Instantiate the fish
        GameObject newFish = Instantiate(fishPrefab, spawnPosition, Quaternion.identity);

        // Scale the fish based on the screen size
        float scaleFactor = Screen.height / baseScreenHeight; // Calculate scale factor
        newFish.transform.localScale *= scaleFactor + 0.7f;

        // Determine the fish's movement direction and speed
        float speed = Random.Range(minSpeed, maxSpeed);
        float direction = spawnFromLeft ? 1f : -1f; // Move right if from left, move left if from right
        Rigidbody2D rb = newFish.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction * speed, 0);

        // Flip the fish sprite if it's swimming left
        if (!spawnFromLeft)
        {
            Vector3 localScale = newFish.transform.localScale;
            localScale.x *= -1; // Flip horizontally
            newFish.transform.localScale = localScale;
        }

        // Add sway motion
        StartCoroutine(SwayFish(newFish, rb));

        // Destroy the fish once it goes offscreen
        Destroy(newFish, 10f);
    }

    IEnumerator SwayFish(GameObject fish, Rigidbody2D rb)
    {
        float elapsedTime = 0f;

        while (fish != null)
        {
            if (rb != null)
            {
                // Add sway to the Y position
                float swayOffset = Mathf.Sin(elapsedTime * swaySpeed) * swayAmount;
                Vector2 currentVelocity = rb.velocity;
                rb.velocity = new Vector2(currentVelocity.x, swayOffset);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
