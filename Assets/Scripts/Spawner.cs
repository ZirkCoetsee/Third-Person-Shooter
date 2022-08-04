using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Wave[] waves;

    Wave currentWave;
    int currentWaveNumber;
    int enemiesRemaingToSpawn;
    int enemiesRemaingAlive;
    float nextSpawnTime;

    public void Start()
    {
        NextWave();
    }

    private void Update()
    {
        if (enemiesRemaingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemaingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            GameObject spawnedEnemy = Instantiate(currentWave.enemy, new Vector3(this.transform.position.x, 0f, this.transform.position.z), Quaternion.identity) as GameObject;
        }
    }

    public void NextWave()
    {
        currentWaveNumber++;
        //Stop out of array exeption if the waves have finished

        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];
            enemiesRemaingToSpawn = currentWave.enemyCount;

            enemiesRemaingAlive = enemiesRemaingToSpawn;
        }
    }

    //Add system.seriablizable to allow class to be seen in inspector
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
        public GameObject enemy;

    }

    public void OnEnmyDeath()
    {
        enemiesRemaingAlive--;
        if (enemiesRemaingAlive == 0)
        {
            NextWave();
        }
    }
}
