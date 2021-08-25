using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] bool looping = true;

    public int Counter;
    public int Players = 2;
    public int Waves = 1;

    // Start is called before the first frame update
    void Start()
    {
        Counter = 0;
        StartCoroutine(SpawnAllWaves());       
    }

    private IEnumerator SpawnAllWaves()
    {
        do
        {
            for (int waveCount = 0; waveCount < waveConfigs.Count; waveCount++)
            {
                var currentWave = waveConfigs[waveCount];
                yield return StartCoroutine(SpawnEnemiesInWave(currentWave));
                Waves++;
            }
            
        }
        while (looping) ;
    }

    private IEnumerator SpawnEnemiesInWave(WaveConfig waveConfig)
    {
        for (int count = 0; count < waveConfig.GetNumberOfEnemies(); count++)
        {
            if (Counter < 25)
            {
                var newEnemy = Instantiate(
                                waveConfig.GetEnemyPrefab(),
                                waveConfig.GetWaypoints()[0].transform.position,
                                Quaternion.identity);

                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                Counter++;
            }
           
            yield return new WaitForSeconds(Mathf.Pow(0.97f,Waves)*(waveConfig.GetTimeBetweenSpawn()));
        }

        yield return new WaitForSeconds(Mathf.Pow(0.935f, Waves) * 3);

    }

}
