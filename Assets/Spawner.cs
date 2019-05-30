using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float spawnInterval = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if(spawnInterval > 0)
        {
            Scheduler.instance.Schedule(spawnInterval, true, Spawn);
        }
    }

    public void Spawn()
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
