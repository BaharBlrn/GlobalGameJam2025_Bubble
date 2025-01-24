using UnityEngine;

public class SwordfishBehavior : MonoBehaviour
{
    private Vector3 targetDirection; // Direction toward the player's location
    private float speed; // Movement speed

    public void Init(Transform player)
    {
        if (player == null)
        {
            Debug.LogWarning("Player transform not assigned to SwordfishBehavior!");
            Destroy(gameObject); // Destroy Swordfish if no player is found
            return;
        }

        // Calculate the direction to the player's current position
        targetDirection = (player.position - transform.position).normalized;
    }

    void Update()
    {
        // Move the Swordfish in the target direction
        transform.position += targetDirection * speed * Time.deltaTime;

        // Rotate the Swordfish to face the movement direction
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Destroy the Swordfish if it moves too far offscreen
        float screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        float screenBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        float screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        if (transform.position.x < screenLeft - 5f || transform.position.x > screenRight + 5f ||
            transform.position.y < screenBottom - 5f || transform.position.y > screenTop + 5f)
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed; // Allow speed to be set dynamically
    }
}
