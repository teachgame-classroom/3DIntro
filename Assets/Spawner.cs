using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float spawnInterval = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Scheduler.instance.Schedule(spawnInterval, true, Spawn);
    }

    void Spawn()
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
