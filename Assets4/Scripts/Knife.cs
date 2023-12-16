using UnityEngine;

public class Knife : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the bullet

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the bullet collided with an enemy (e.g., tagged as "Zombie")
        if (other.CompareTag("Zombie"))
        {
            Debug.Log("Damage: " + damage); // Add this line for debugging
            // Deal damage to the enemy and destroy the bullet
            ZombieHealth enemyHealth = other.GetComponent<ZombieHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}