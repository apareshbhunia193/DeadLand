using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] int number;
    [SerializeField] float spawnRadius;
    [SerializeField] bool spawnOnStart = true;
    bool alreadySpawn = false;
    // Start is called before the first frame update
    void Start()
    {
        if (spawnOnStart)
            SpawnAll();
    }

    void SpawnAll() {
        for (int i = 0; i < number; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
            {
                Instantiate(zombiePrefab, hit.position, Quaternion.identity);
            }
            else
                i--;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!spawnOnStart && other.gameObject.CompareTag("Player") && !alreadySpawn)
        {
            SpawnAll();
            alreadySpawn = true;
        }

    }
}
