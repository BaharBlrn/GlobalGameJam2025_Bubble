using UnityEngine;

public class PiranhaBehavior : MonoBehaviour
{
    private Transform player; // Reference to the player's transform
    private float followDuration; // How long the Piranha will follow the player
    private float speed; // Piranha's movement speed
    private float elapsedTime = 0f; // Timer to track follow duration
    private bool isChasing = true; // Flag to track whether the Piranha is chasing

    public void Init(float followDuration, float speed)
    {
        this.followDuration = followDuration;
        this.speed = speed;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogWarning("No GameObject tagged as 'Player' found!");
            Destroy(gameObject); // Destroy the Piranha if no player is found
        }
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            MoveOffscreen();
        }

        elapsedTime += Time.deltaTime;

        // Stop chasing after followDuration
        if (elapsedTime >= followDuration && isChasing)
        {
            isChasing = false;
        }
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            // Move toward the player
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Rotate the Piranha to face the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void MoveOffscreen()
    {
        // Continue moving in the current direction (based on the last chase direction)
        transform.position += transform.right * speed * Time.deltaTime;

        // Optionally destroy the Piranha after it's far enough offscreen
        float screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        float screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        float screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        if (transform.position.x < screenLeft - 5f || transform.position.x > screenRight + 5f ||
            transform.position.y < screenBottom - 5f || transform.position.y > screenTop + 5f)
        {
            Destroy(gameObject); // Destroy the Piranha after it moves far enough offscreen
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the Piranha touches the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Piranha touched the player!");
            // Add any effects (e.g., damage, status effects)
            Destroy(gameObject); // Destroy the Piranha after touching the player
        }
    }
}
