using UnityEngine;

public class Testing : MonoBehaviour
{
    public float moveSpeed = 5f; // Player movement speed

    private void Update()
    {
        // Get input for player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction based on input
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

        // Update the player's position
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}