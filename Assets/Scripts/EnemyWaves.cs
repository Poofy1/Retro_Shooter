using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{

    [SerializeField] private Transform enemyTransform;

    void Start()
    {
        StartWave();
    }

    private void StartWave()
    {
        Debug.Log("Wave Spawned");
        //enemyTransform.GetComponent<EnemySpawn>().Spawn();
    }
}