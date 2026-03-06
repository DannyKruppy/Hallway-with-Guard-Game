using UnityEngine;

public class TumbleweedSpawner : MonoBehaviour
{
    public GameObject tumbleweedPrefab;     // Prefab to spawn
    public float spawnHeight = 5f;          // Height above player to start ground raycast
    public float spawnDistanceAhead = 3f;   // How far in front of the player to spawn
    public float cooldown = 2f;             // Minimum time between spawns

    private bool canSpawn = true;           // Tracks if tumbleweed can spawn (cooldown)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canSpawn)
        {
            // Only spawn if collider is the player AND cooldown allows 
            SpawnTumbleweed(other.transform);   // Spawn tumbleweed ahead of player 
            StartCoroutine(SpawnCooldown());    // Start cooldown timer
        }
    }

    void SpawnTumbleweed(Transform player)
    {
        // Spawn a little ahead of the player by calculating offset in front of player
        Vector3 forwardOffset = player.forward * spawnDistanceAhead;

        // Starting raycast position above player
        Vector3 startPos = player.position + forwardOffset + Vector3.up * spawnHeight;

        // Raycast downward to find the ground position
        if (Physics.Raycast(startPos, Vector3.down, out RaycastHit hit, 20f))
        {
            // Instantiate tumbleweed
            GameObject tumbleweed = Instantiate(tumbleweedPrefab);

            // Get vertical radius of collider to place it properly on ground
            float radius = tumbleweed.GetComponent<Collider>().bounds.extents.y;

            // Move tumbleweed to the ground hit point plus radius offset
            tumbleweed.transform.position = hit.point + Vector3.up * radius;

            // Randomize rotation for natural tumbleweed look 
            tumbleweed.transform.rotation = Random.rotation;
        }
    }

    System.Collections.IEnumerator SpawnCooldown()
    {
        canSpawn = false;   // Disable spawning
        yield return new WaitForSeconds(cooldown);  // Wait for cooldown duration
        canSpawn = true;    // Re-enable spawning
    }
}