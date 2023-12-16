using UnityEngine;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float shootInterval = 2f; // Time interval between shots
    public float bulletSpeed = 5f;

    private float shootTimer = 0f;
    private bool moveRight = true;

    private void Update()
    {
        Move();

        // Shoot bullets at the player
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void Move()
    {
        Vector3 movement = moveRight ? Vector3.right : Vector3.left;
        transform.Translate(movement * (moveSpeed * Time.deltaTime));
    }

    void Shoot()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            // Set velocity in the y-axis to make bullets move downward
            bulletRb.velocity = Vector2.down * bulletSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reverse direction on collision with a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveRight = !moveRight;
        }
    }
}
