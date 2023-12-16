using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseHealth2 : MonoBehaviour
{
    public int maxHealth = 10; // Maximum health of the base
    private int _currentHealth; // Current health of the base
    private SpriteRenderer _spriteRenderer; // Reference to the SpriteRenderer component

    public Color fullHealthColor = Color.green; // Color when base has full health
    public Color damagedColor = Color.yellow; // Color when base has 2 or 1 hp left
    public Color criticalColor = Color.red; // Color when base has 1 hp left

    private void Start()
    {
        _currentHealth = maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial color to fullHealthColor
        _spriteRenderer.color = fullHealthColor;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        // Change the base's color based on current health
        if (_currentHealth >= 2)
        {
            _spriteRenderer.color = damagedColor;
        }
        else if (_currentHealth == 1)
        {
            _spriteRenderer.color = criticalColor;
        }
        else if (_currentHealth <= 0)
        {
            // Base is destroyed or game over logic here
            Debug.Log("Base destroyed!");

            // Load the game over scene (you should replace "GameOverScene" with your actual scene name)
            SceneManager.LoadScene("GameOver2");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged as "Zombie"
        if (collision.gameObject.CompareTag("Zombie"))
        {
            // Deduct damage from the base's health when colliding with a zombie
            TakeDamage(1); // Deduct 1 health point

            // Destroy the zombie GameObject
            Destroy(collision.gameObject);
        }
    }
}