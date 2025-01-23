using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f; // Speed of player movement
    public int maxHP = 10; // Maximum HP of the player
    public float invulnerabilityDuration = 1f; // Duration of invulnerability after being hit

    [Header("UI/Stats")]
    public int currentHP; // Current HP of the player
    public int bubblesCollected = 0; // Number of bubbles collected

    private bool isInvulnerable = false; // Tracks if the player is invulnerable
    private SpriteRenderer spriteRenderer; // For flashing effect during invulnerability

    void Start()
    {
        // Initialize HP and components
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Handle player movement
        MovePlayer();
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle bubble collection
        if (collision.gameObject.CompareTag("Bubble"))
        {
            CollectBubble(collision.gameObject);
        }

        // Handle collision with fish
        if (collision.gameObject.CompareTag("Fish") && !isInvulnerable)
        {
            TakeDamage(1);
        }
    }

    private void CollectBubble(GameObject bubble)
    {
        bubblesCollected++; // Increase bubble currency
        Destroy(bubble); // Destroy the bubble object
        Debug.Log($"Bubble collected! Total: {bubblesCollected}");
    }

    private void TakeDamage(int damage)
    {
        currentHP -= damage; // Decrease HP
        Debug.Log($"Player hit! Current HP: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeInvulnerable());
        }
    }

    private System.Collections.IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;

        // Flashing effect for invulnerability
        float elapsedTime = 0f;
        while (elapsedTime < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle visibility
            yield return new WaitForSeconds(0.1f); // Flash interval
            elapsedTime += 0.1f;
        }

        spriteRenderer.enabled = true; // Ensure visibility is restored
        isInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // Add game over logic here
        // Example: Reload the scene or show a game over screen
    }
}
