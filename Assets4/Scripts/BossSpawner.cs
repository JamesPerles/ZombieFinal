using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    public GameObject characterPrefab; // The character prefab to spawn
    public float spawnDelay = 40f; // Time delay before spawning (in seconds)

    private float elapsedTime = 0f;
    private bool hasSpawned = false;

    private void Update()
    {
        if (!hasSpawned)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= spawnDelay)
            {
                SpawnCharacter();
                hasSpawned = true;
            }
        }
    }

    void SpawnCharacter()
    {
        if (characterPrefab != null)
        {
            Instantiate(characterPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Character prefab not assigned in the inspector.");
        }
    }
}