using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public float speed = 2f; // Zombie movement speed
    public int damage = 1; // Damage dealt to the base

    private Transform _target; // Target (the base) for the zombie to move towards
    private bool _hasReachedBase; // To prevent multiple damage applications

    private void Start()
    {
        // Find the GameObject with the "Base" tag
        _target = GameObject.FindGameObjectWithTag("Base").transform;

        if (_target == null)
        {
            Debug.LogError("Base not found. Make sure you have a GameObject with the 'Base' tag.");
        }
    }

    private void Update()
    {
        if (_target != null && !_hasReachedBase)
        {
            // Calculate the direction from the zombie to the base
            Vector3 direction = (_target.position - transform.position).normalized;

            // Move the zombie towards the base using Rigidbody2D
            GetComponent<Rigidbody2D>().velocity = direction * speed;

            // Adjust the rotation of the zombie based on its movement direction
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            // Check for collision with the base
            float distanceToBase = Vector3.Distance(transform.position, _target.position);
            if (distanceToBase < 0.1f)
            {
                // Zombie has reached the base, deal damage and set the flag
                BaseHealth baseHealth = _target.GetComponent<BaseHealth>();
                if (baseHealth != null)
                {
                    baseHealth.TakeDamage(damage);
                }
                _hasReachedBase = true;

                // Destroy the zombie
                Destroy(gameObject);
            }
        }
    }
}

