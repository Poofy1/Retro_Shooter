using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

[System.Serializable]


public class Wave
{
    public string waveName;
    public int noOfEnemies;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;

}

public class WaveSystem : MonoBehaviour
{

    [SerializeField] Wave[] waves;
    public Transform[] spawnPoints;

    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;

    private bool canSpawn = true;


    private bool openedWave;
    public float restTime;
    private float tempTime;
    public float closingWait;
    private float tempClose;

    float currentTime;

    public Animator doors;
    public TextMeshPro doorTimer;


    void Start()
    {
        openedWave = false;
        tempTime = restTime;
        tempClose = closingWait;
        doorTimer.text = "";
    }

    private void Update()
    {
        currentWave = waves[currentWaveNumber];
        SpawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(totalEnemies.Length == 0 && !canSpawn && currentWaveNumber+1 != waves.Length)
        {
            currentWaveNumber++;
            canSpawn = true;
            openedWave = false;
        }

        if (!openedWave)
        {
            tempTime -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(tempTime);
            doorTimer.text = String.Format(@"{0:ss\.' 'ff}", time);
            if (tempTime < 0)
                StartWave();
        }
        else
        {
            tempClose -= Time.deltaTime;
            Debug.Log("Close:"+tempClose);
            if (tempClose < 0)
                CloseDoor();
        }
    }

    private void StartWave()
    {
        doorTimer.text = "";
        openedWave = true;
        doors.SetBool("Close", false);
        doors.SetBool("Open", true);
        Debug.Log("Open");
    }

    private void CloseDoor()
    {
        //When Wave killed:
        //openedWave = false

        tempClose = closingWait;
        doors.SetBool("Open", false);
        doors.SetBool("Close", true);
        Debug.Log("Close");
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            currentWave.noOfEnemies--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (currentWave.noOfEnemies == 0)
            {
                canSpawn = false;
            }
        }

    }

}
