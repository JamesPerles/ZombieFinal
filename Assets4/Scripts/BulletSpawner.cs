using UnityEngine;
using UnityEngine.UI;

namespace Assets4.Scripts
{
    public class BulletSpawner : MonoBehaviour
    {
        public GameObject bulletPrefab; // The bullet GameObject to spawn
        public Transform bulletSpawnPoint; // The point from which bullets are spawned
        public float bulletSpeed = 10f; // The speed of the bullets
        public int maxBullets = 10; // Maximum number of bullets you can carry
        public int bulletsInInventory; // Current bullets in the inventory
        public string ammoTag = "Ammo"; // Tag for ammo refill objects
        public int ammoRefillAmount = 5; // Amount of bullets to refill on collision with ammo
        public Text bulletCountText; // Reference to the UI Text element to display bullet count

        void Start()
        {
            bulletsInInventory = maxBullets; // Initialize with the maximum number of bullets
            UpdateBulletCountText(); // Update the bullet count UI
        }

        void Update()
        {
            // Check if the spacebar is pressed and you have bullets in the inventory
            if (Input.GetKeyDown(KeyCode.Space) && bulletsInInventory > 0)
            {
                // Spawn a bullet and reduce the bullet count
                SpawnBullet();
                bulletsInInventory--;
                UpdateBulletCountText(); // Update the bullet count UI
            }
        }

        void SpawnBullet()
        {
            // ... (Rest of your code remains unchanged)
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Check for collision with objects tagged as "Ammo"
            if (other.CompareTag(ammoTag))
            {
                // Refill bulletsInInventory by the specified amount
                bulletsInInventory = Mathf.Min(maxBullets, bulletsInInventory + ammoRefillAmount);
                Destroy(other.gameObject); // Destroy the ammo object
                UpdateBulletCountText(); // Update the bullet count UI
            }
        }

        // Function to update the UI Text with the remaining bullet count
        void UpdateBulletCountText()
        {
            if (bulletCountText != null)
            {
                bulletCountText.text = "Bullets: " + bulletsInInventory.ToString();
            }
        }
    }
}
