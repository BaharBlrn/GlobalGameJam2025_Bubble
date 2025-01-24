using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f; // Horizontal movement speed of the fish
    public float swayAmount = 0.5f; // Amplitude of the sway (up and down motion)
    public float swaySpeed = 2f; // Speed of the sway (frequency of the sinusoidal motion)

    private Vector2 direction;
    private float initialY; // To store the initial Y position

    void Start()
    {
        // Determine movement direction based on the spawn position's X coordinate
        direction = transform.position.x < 0 ? Vector2.right : Vector2.left;

        // Store the initial Y position
        initialY = transform.position.y;

        // Destroy the fish after 5 seconds
        Destroy(gameObject, 25f);
    }

    void Update()
    {
        // Calculate the sway offset using a sine wave
        float swayOffset = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        // Apply horizontal movement and vertical sway
        Vector2 newPosition = new Vector2(transform.position.x + direction.x * speed * Time.deltaTime, initialY + swayOffset);
        transform.position = newPosition;
    }
}
