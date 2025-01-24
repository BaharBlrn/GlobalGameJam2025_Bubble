using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<Wave> waves; // Define waves in the Inspector
    public float timeBetweenWaves = 10f; // Delay between each wave
    public Transform player; // Reference to the player object

    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    void Start()
    {
        if (waves == null || waves.Count == 0)
        {
            Debug.LogWarning("No waves defined in WaveManager!");
            return;
        }

        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            if (!isSpawning)
            {
                isSpawning = true;
                StartCoroutine(SpawnWave(waves[currentWaveIndex]));
                currentWaveIndex++;
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (WaveEntry entry in wave.entries)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                // Spawn fish at defined positions
                Vector3 spawnPosition = entry.spawnPositions[i % entry.spawnPositions.Count];
                GameObject fish = Instantiate(entry.fishPrefab, spawnPosition, Quaternion.identity);

                // Initialize fish behavior if applicable
                if (fish.TryGetComponent<SwordfishBehavior>(out SwordfishBehavior swordfish))
                {
                    swordfish.Init(player);
                }
                else if (fish.TryGetComponent<PiranhaBehavior>(out PiranhaBehavior piranha))
                {
                    piranha.Init(10f, entry.speed); // 10 seconds chase duration and defined speed
                }

                yield return new WaitForSeconds(entry.spawnInterval); // Delay between spawns within the wave
            }
        }

        isSpawning = false;
    }
}

[System.Serializable]
public class Wave
{
    public string waveName; // Name for the wave (optional)
    public List<WaveEntry> entries; // Define each fish type and spawn parameters for this wave
}

[System.Serializable]
public class WaveEntry
{
    public GameObject fishPrefab; // The type of fish to spawn
    public List<Vector3> spawnPositions; // Predefined spawn positions for this fish type
    public int amount; // Number of this fish type to spawn
    public float spawnInterval; // Time between spawns of this fish type
    public float speed; // Speed of the fish (used for Piranhas)
}
