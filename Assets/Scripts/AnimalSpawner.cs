using UnityEngine;
using System;                      // for Action
using Random = UnityEngine.Random; // avoid Random ambiguity

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] animals;
    public Transform[] spawnPoints;

    public static event Action OnAnimalSpawned;

    public void SpawnAnimal()
    {
        if (animals == null || animals.Length == 0)
        {
            Debug.LogWarning("[Spawner] No animals assigned!");
            return;
        }
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[Spawner] No spawn points assigned!");
            return;
        }

        var prefab = animals[Random.Range(0, animals.Length)];
        var point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(prefab, point.position, point.rotation);

        OnAnimalSpawned?.Invoke();
        Debug.Log($"[Spawner] Spawned: {prefab.name} @ {point.name}");
    }
}



