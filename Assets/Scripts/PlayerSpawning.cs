using UnityEngine;

public class PlayerSpawning : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerInstance; // Reference to the player instance in the scene
    [SerializeField] Transform spawnPoint;
    [SerializeField] float killZoneY = -10f; // Y position below which the player will be reset to the spawn point

    private void Start()
    {
        InitialPlayerSpawn();
    }

    void InitialPlayerSpawn()
    {
        if (playerPrefab != null && spawnPoint != null)
        {
            playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Player prefab or spawn point not assigned in PlayerSpawning script.");
        }
    }

    void ResetPlayer(GameObject player)
    {
        if (spawnPoint != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; // Reset velocity to prevent carryover of momentum
            }
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
        }
        else
        {
            Debug.LogError("Spawn point not assigned in PlayerSpawning script.");
        }
    }

    private void Update()
    {
        if (playerInstance != null && playerInstance.transform.position.y < killZoneY)
        {
            ResetPlayer(playerInstance);
        }
    }
}
