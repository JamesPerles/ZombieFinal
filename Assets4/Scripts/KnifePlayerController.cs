using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets4.Scripts
{
    public class KnifePlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        float rotationOffset = 0f;
        public GameObject knifeSlashPrefab;
        public Transform knifeSpawnPoint;
        public float slashDuration = 1f;
        public float knifeSpawnOffset = 0.5f;
        private Vector2 _moveDirection;
        private bool _isHit = false;
        [SerializeField] private GameObject normalPlayerPrefab;
        [SerializeField] private Transform normalPlayerSpawnPoint;
        private int initialBullets = 5;

        private void Update()
        {
            if (!_isHit)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");

                _moveDirection = new Vector2(horizontalInput, verticalInput).normalized;

                if (_moveDirection != Vector2.zero)
                {
                    float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg + rotationOffset;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine(SpawnKnifeSlash());
                }
            }
        }

        private IEnumerator SpawnKnifeSlash()
        {
            if (knifeSlashPrefab != null && knifeSpawnPoint != null)
            {
                Vector3 spawnPosition = knifeSpawnPoint.position + knifeSpawnPoint.right * knifeSpawnOffset;

                GameObject knifeSlash = Instantiate(knifeSlashPrefab, spawnPosition, knifeSpawnPoint.rotation);
                knifeSlash.transform.parent = knifeSpawnPoint;

                yield return new WaitForSeconds(slashDuration);

                Destroy(knifeSlash);
            }
            else
            {
                Debug.LogWarning("Knife slash prefab or spawn point not assigned in the inspector.");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ammo") && !_isHit)
            {
                SwitchToNormalPlayer();
                Destroy(gameObject);
            }
            else if (other.CompareTag("ZombieBullet") && !_isHit)
            {
                StartCoroutine(HitEffect());
            }
        }

        private void FixedUpdate()
        {
            if (!_isHit)
            {
                GetComponent<Rigidbody2D>().velocity = _moveDirection * moveSpeed;
            }
        }

        IEnumerator HitEffect()
        {
            _isHit = true;
            moveSpeed = 0f;
            yield return new WaitForSeconds(5f);
            moveSpeed = 5f;
            _isHit = false;
        }

        private void SwitchToNormalPlayer()
        {
            if (normalPlayerPrefab != null && normalPlayerSpawnPoint != null)
            {
                // Instantiate the normal player at the spawn point
                GameObject player = Instantiate(normalPlayerPrefab, normalPlayerSpawnPoint.position, normalPlayerSpawnPoint.rotation);

                // Destroy the knife player script
                Destroy(gameObject);

                // Set the player's position to the knife player's position
                player.transform.position = transform.position;

                // Activate the player
                player.SetActive(true);

                // Set the initial bullets for the normal player
                player.GetComponent<PlayerController>().bulletsInInventory = initialBullets;
            }
            else
            {
                Debug.LogError("Normal player prefab or spawn point not assigned in the inspector.");
            }
        }
    }
}
