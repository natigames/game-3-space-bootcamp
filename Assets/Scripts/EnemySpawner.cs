using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;


    // Start is called before the first frame update
    //MAKE START A COROUTINE! AWESOME
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
    }


    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesinWave(currentWave));
        }

    }

    //coroutine to spawn
    private IEnumerator SpawnAllEnemiesinWave(WaveConfig waveConfig)
    {
        //  Loop Number of Enemies...
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            // what, where and rotation
            var newEnemy = 
                Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWayPoints()[0].transform.position,
                    Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);


            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawn());
        }
    }

}
