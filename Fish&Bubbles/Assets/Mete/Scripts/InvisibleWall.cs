using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player hit the invisible wall!");

            // Get the Rigidbody2D of the player
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Stop the player's movement
                playerRb.velocity = Vector2.zero;

                // Optionally move the player slightly back to prevent overlap
                Vector2 pushBack = (collision.transform.position - transform.position).normalized * 0.1f;
                collision.transform.position += (Vector3)pushBack;
            }
        }
    }
}
