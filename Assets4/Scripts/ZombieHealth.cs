using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10; // Maximum health of the zombie
    [SerializeField] private int _currentHealth; // Current health of the zombie
    public AudioClip destroySound; // Sound to play when the zombie is destroyed
    private AudioSource _audioSource; // Reference to the AudioSource component

    public int CurrentHealth => _currentHealth; // Property to access current health

    private void Start()
    {
        _currentHealth = maxHealth;

        // Get the AudioSource component attached to the zombie or add one if not present
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Make sure the AudioSource is not null
        if (_audioSource != null)
        {
            // Disable play on awake to prevent automatic playback
            _audioSource.playOnAwake = false;
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            // Zombie is dead, play the destroy sound
            PlayDestroySound();

            // Destroy the zombie
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged as "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Apply damage to the zombie
            TakeDamage(1); // Assuming each bullet deals 1 damage

            // Destroy the bullet
            Destroy(collision.gameObject);
        }
    }

    private void PlayDestroySound()
    {
        Debug.Log("Playing destroy sound");

        // Check if a destroy sound is assigned and the AudioSource is not null
        if (destroySound != null && _audioSource != null)
        {
            // Enable the AudioSource if it's disabled
            if (!_audioSource.enabled)
            {
                _audioSource.enabled = true;
            }

            // Play the destroy sound
            _audioSource.PlayOneShot(destroySound);
        }
        else
        {
            Debug.LogWarning("Destroy sound not assigned or AudioSource not in the correct state.");
            Debug.Log($"destroySound: {destroySound}, audioSource: {_audioSource}");
        }
    }
}

