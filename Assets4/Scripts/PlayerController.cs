using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets4.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f; // Player movement speed
        public GameObject bulletPrefab; // The bullet GameObject to spawn
        public Transform bulletSpawnPoint; // The point from which bullets are spawned
        public float bulletSpeed = 10f; // The speed of the bullets
        public int maxBullets = 10; // Maximum number of bullets you can carry
        public int bulletsInInventory; // Current bullets in the inventory
        public string ammoTag = "Ammo"; // Tag for ammo refill objects
        public int ammoRefillAmount = 5; // Amount of bullets to refill on collision with ammo
        public TextMeshProUGUI bulletCountText; // Reference to the TextMeshPro Text element to display bullet count
        public float rotationOffset; // Rotation offset to customize the rotation when the direction changes
        public GameObject knifePlayerPrefab; // The prefab of the knife-wielding player
        public Transform knifeSpawnPoint; // The point from which the knife player is spawned
        public AudioSource bulletSpawnSound; // Reference to the AudioSource for bullet spawn sound
        private Vector2 _moveDirection; // Direction of player movement
        private bool _isHit; // Flag to check if the player is hit
        private GameObject _currentPlayer; // Reference to the current player object

        private void Start()
        {
            bulletsInInventory = maxBullets; // Initialize with the maximum number of bullets
            UpdateBulletCountText(); // Update the bullet count UI

            // Instantiate the knife-wielding player initially but deactivate it
            InstantiateKnifePlayer();
            SwitchPlayer(gameObject); // Set the current player to the original player

            // Debugging statements
            Debug.Log("Start: Current Player: " + _currentPlayer.name);
            Debug.Log("Start: Knife Spawn Point: " + knifeSpawnPoint.name);
            Debug.Log("Start: Knife Player Active: " + knifeSpawnPoint.GetChild(0).gameObject.activeSelf);
            Debug.Log("Start: Knife Player Position: " + knifeSpawnPoint.GetChild(0).position);
        }

        private void Update()
        {
            if (!_isHit)
            {
                // Get input for player movement
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");

                // Calculate the movement direction based on input
                _moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

                // Update player rotation based on the movement direction
                if (_moveDirection != Vector2.zero)
                {
                    float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg + rotationOffset;
                    _currentPlayer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }

                // Check if the spacebar is pressed and you have bullets in the inventory
                if (Input.GetKeyDown(KeyCode.Space) && bulletsInInventory > 0)
                {
                    // Spawn a bullet in front of the player
                    SpawnBullet();
                    bulletsInInventory--;
                    UpdateBulletCountText(); // Update the bullet count UI
                }
                else if (Input.GetKeyDown(KeyCode.Space) && bulletsInInventory == 0)
                {
                    // Switch to the knife-wielding player if no bullets are left
                    SwitchToKnifePlayer();
                }
            }
        }

        void SpawnBullet()
        {
            if (bulletPrefab != null && bulletSpawnPoint != null)
            {
                var position = bulletSpawnPoint.position;
                GameObject bullet = Instantiate(bulletPrefab, position, bulletSpawnPoint.rotation);

                // Set the parent to null to avoid inheriting the rotation
                bullet.transform.parent = null;

                // Play the bullet spawn sound
                if (bulletSpawnSound != null)
                {
                    bulletSpawnSound.Play();
                }

                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

                // Set the velocity based on the bulletSpawnPoint's up direction
                bulletRb.velocity = bulletSpawnPoint.up * bulletSpeed;
            }
            else
            {
                Debug.LogWarning("Bullet prefab or spawn point not assigned in the inspector.");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(ammoTag))
            {
                bulletsInInventory = Mathf.Min(maxBullets, bulletsInInventory + ammoRefillAmount);
                Destroy(other.gameObject);
                UpdateBulletCountText();
            }
            else if (other.CompareTag("ZombieBullet") && !_isHit)
            {
                // Reduce player's movement to zero for 5 seconds
                StartCoroutine(HitEffect());
            }
        }

        void UpdateBulletCountText()
        {
            if (bulletCountText != null)
            {
                bulletCountText.text = "Bullets: " + bulletsInInventory.ToString();
            }
        }

        private void FixedUpdate()
        {
            if (!_isHit)
            {
                _currentPlayer.GetComponent<Rigidbody2D>().velocity = _moveDirection * moveSpeed;
            }
        }

        // Coroutine for the hit effect
        IEnumerator HitEffect()
        {
            _isHit = true;

            // Add any visual effect or sound for hit if needed

            // Reduce movement to zero
            moveSpeed = 0f;

            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);

            // Reset movement speed and hit flag
            moveSpeed = 5f;
            _isHit = false;
        }

        // Instantiate the knife-wielding player and set its initial state
        void InstantiateKnifePlayer()
        {
            if (knifePlayerPrefab != null && knifeSpawnPoint != null)
            {
                GameObject knifePlayer = Instantiate(knifePlayerPrefab, knifeSpawnPoint.position, knifeSpawnPoint.rotation);
                knifePlayer.transform.SetParent(knifeSpawnPoint); // Set the knife player as a child of the spawn point
                knifePlayer.SetActive(false); // Deactivate the knife player initially
            }
            else
            {
                Debug.LogWarning("Knife player prefab or spawn point not assigned in the inspector.");
            }
        }

        // Switch to the knife-wielding player
        void SwitchToKnifePlayer()
        {
            // Deactivate the current player
            _currentPlayer.SetActive(false);

            // Activate the knife player
            Transform knifePlayerTransform = knifeSpawnPoint.GetChild(0);
            GameObject knifePlayer = knifePlayerTransform != null ? knifePlayerTransform.gameObject : null;
            if (knifePlayer != null)
            {
                knifePlayer.SetActive(true);

                // Set the position and rotation of the knife player to match the original player
                knifePlayer.transform.position = _currentPlayer.transform.position;
                knifePlayer.transform.rotation = _currentPlayer.transform.rotation;

                // Set the current player to the knife player
                _currentPlayer = knifePlayer;
            }
        }

        // Switch the current player between the original and knife-wielding player
        void SwitchPlayer(GameObject newPlayer)
        {
            // Deactivate the current player
            if (_currentPlayer != null)
            {
                _currentPlayer.SetActive(false);
            }

            // Activate the new player
            newPlayer.SetActive(true);

            // Set the current player to the new player
            _currentPlayer = newPlayer;
        }
    }
}
