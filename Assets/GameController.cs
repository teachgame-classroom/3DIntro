using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int killCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void OnEnemyKilled()
    {
        killCount++;
        Debug.Log(killCount);
        if(killCount == 20)
        {
            GameObject.Find("BossSpawner").GetComponent<Spawner>().Spawn();
            GameObject.Find("CameraRoot").GetComponent<CameraFollow>().StartLookAt();
        }
    }


}
