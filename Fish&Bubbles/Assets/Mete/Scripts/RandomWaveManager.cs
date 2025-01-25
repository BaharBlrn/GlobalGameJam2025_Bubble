using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<Wave> waves; // Pre-defined waves
    public float timeBetweenWaves = 10f; // Delay between each wave
    public Transform player; // Reference to the player object

    [Header("Game Settings")]
    public float speedIncreasePerWave = 0.1f; // How much to increase game speed per wave
    public PlayerController playerController; // Reference to the PlayerController script
    public UnityEngine.UI.Text timeSurvivedText; // Reference to UI text for displaying time survived

    private bool isSpawning = false;
    private float timeSurvived = 0f;
    private float currentGameSpeed = 1f;
    private bool gameActive = true;

    void Start()
    {
        if (waves == null || waves.Count == 0)
        {
            Debug.LogWarning("No waves defined in WaveManager!");
            return;
        }

        if (playerController == null)
        {
            Debug.LogError("PlayerController reference is not set!");
            return;
        }

        StartCoroutine(SpawnWavesInfinitely());
        StartCoroutine(TrackTimeSurvived());
    }

    IEnumerator SpawnWavesInfinitely()
    {
        while (gameActive)
        {
            if (!isSpawning)
            {
                isSpawning = true;

                // Select a random wave from the predefined waves
                Wave randomWave = waves[Random.Range(0, waves.Count)];

                // Spawn the wave
                yield return StartCoroutine(SpawnWave(randomWave));

                // Increase game speed after the wave finishes
                currentGameSpeed += speedIncreasePerWave;

                yield return new WaitForSeconds(timeBetweenWaves / currentGameSpeed); // Delay before the next wave
            }
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        foreach (WaveEntry entry in wave.entries)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                // Spawn fish at the pre-defined positions in the wave
                Vector3 spawnPosition = entry.spawnPositions[i % entry.spawnPositions.Count];
                GameObject fish = Instantiate(entry.fishPrefab, spawnPosition, Quaternion.identity);

                // Initialize fish behavior if applicable
                if (fish.TryGetComponent<SwordfishBehavior>(out SwordfishBehavior swordfish))
                {
                    swordfish.Init(player, entry.speed * currentGameSpeed); // Adjust speed
                }
                else if (fish.TryGetComponent<PiranhaBehavior>(out PiranhaBehavior piranha))
                {
                    piranha.Init(10f, entry.speed * currentGameSpeed); // Adjust speed
                }

                yield return new WaitForSeconds(entry.spawnInterval / currentGameSpeed); // Delay between spawns
            }
        }

        isSpawning = false;
    }

    IEnumerator TrackTimeSurvived()
    {
        while (gameActive)
        {
            // Check player's health from PlayerController
            if (playerController.currentHP <= 0)
            {
                gameActive = false;
                Debug.Log($"Game Over! You survived for {timeSurvived:F2} seconds.");
            }

            timeSurvived += Time.deltaTime;
            if (timeSurvivedText != null)
            {
                timeSurvivedText.text = $"Time Survived: {timeSurvived:F2} s";
            }

            yield return null;
        }
    }
}
