using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpawnableObject
{
    public GameObject prefab;
    [Range(0f, 1f)] public float spawnChance;
}

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<SpawnableObject> spawnables;
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float spawnAbovePlayerY = 3f;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float minDistanceBetweenSpawns = 1f;

    private Transform player;
    private List<Vector3> recentSpawnPositions = new List<Vector3>();

    private void Start()
    {
        player = FindFirstObjectByType<Player>().transform;
        InvokeRepeating(nameof(Spawn), 2f, spawnInterval);
    }

    private void Spawn()
    {
        Vector3 spawnPos;
        int safety = 0;
        do
        {
            float randomX = Random.Range(minX, maxX);
            float randomYOffset = Random.Range(0f, 2f);
            float spawnY = player.position.y + spawnAbovePlayerY + randomYOffset;

            spawnPos = new Vector3(randomX, spawnY, 0f);
            safety++;
            if (safety > 20) break;
        }
        while (TooCloseToOthers(spawnPos));

        GameObject prefabToSpawn = GetRandomPrefab();
        if (prefabToSpawn != null)
        {
            GameObject spawned = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            recentSpawnPositions.Add(spawnPos);
            StartCoroutine(RemoveFromRecent(spawnPos, 2f));
        }
    }

    private GameObject GetRandomPrefab()
    {
        float total = 0f;
        foreach (var obj in spawnables)
        {
            total += obj.spawnChance;
        }

        float randomValue = Random.value * total;
        float cumulative = 0f;

        foreach (var obj in spawnables)
        {
            cumulative += obj.spawnChance;
            if (randomValue <= cumulative)
                return obj.prefab;
        }

        return spawnables[0].prefab; // fallback
    }

    private bool TooCloseToOthers(Vector3 pos)
    {
        foreach (var other in recentSpawnPositions)
        {
            if (Vector3.Distance(pos, other) < minDistanceBetweenSpawns)
                return true;
        }
        return false;
    }

    private IEnumerator RemoveFromRecent(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        recentSpawnPositions.Remove(pos);
    }
}