using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObjects : MonoBehaviour
{
    [SerializeField]
    List<GameObject> Objects;

    [SerializeField] float spawnRate;
    [SerializeField] int maxSpawn;

    int currentSpawned;
    IEnumerator SpawnRate() 
    {
        yield return new WaitForSecondsRealtime(spawnRate);
        SpawnObject();
    }
    private void Update()
    {
        if (currentSpawned < maxSpawn) 
        {
            StartCoroutine(SpawnRate());
        }
    }

    void SpawnObject()
    {
        System.Random rand = new();
        Instantiate(Objects[rand.Next(Objects.Count)], transform.position,Quaternion.identity,gameObject.transform);
        currentSpawned++;
    }
}
