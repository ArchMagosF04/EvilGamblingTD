using Alchemy.Inspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyWavesManager : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private List<SO_EnemyData> availableEnemies;
    [BoxGroup("Components"), SerializeField] private SO_EnemyWavesBlueprint wavesBlueprint;

    [BoxGroup("Spawn Area"), SerializeField] private Transform spawnPoint;
    [BoxGroup("Spawn Area"), SerializeField] private Vector2 offsetRange;

    [BoxGroup("Budget Scaling Settings"), SerializeField, Tooltip("Starting budget for wave 1")] 
    private float baseBudget = 10f;
    [BoxGroup("Budget Scaling Settings"), SerializeField, Tooltip("How much budget increases per wave (linear multiplier)")] 
    private float budgetGrowthMultiplier = 1.3f;
    [BoxGroup("Budget Scaling Settings"), SerializeField, Tooltip("Additional flat budget added per wave")] 
    private float budgetStep = 5f;

    [BoxGroup("Spawn Timing Settings"), SerializeField]
    private float baseSpawnInterval = 2.0f;
    [BoxGroup("Spawn Timing Settings"), SerializeField, Tooltip("Minimum allowed spawn interval to prevent instant crashing overflows")]
    private float minSpawnInterval = 0.2f;
    [BoxGroup("Spawn Timing Settings"), SerializeField, Tooltip("How much faster enemies spawn each wave"), Range(0.1f, 0.9999f)]
    private float intervalDecayRate = 0.95f;

    [SerializeField, ReadOnly] private Queue<SpawnEntry> currentWaveSpawnQueue = new Queue<SpawnEntry>();

    public int CurrentWave { get; private set; } = 0;
    private bool allEnemiesInWaveDead = true;
    private bool waveInProgress;
    private bool spawingInProgess;
    private float lastSpawnTime;

    private int activeEnemiesAmount = 0;

    private float currentWaveBudget;
    private float currentSpawnInterval;

    [Button]
    public void StartNextWave()
    {
        if (allEnemiesInWaveDead)
        {
            CurrentWave++;
            currentWaveBudget = CalculateWaveBudget(CurrentWave);
            currentSpawnInterval = CalculateSpawnInterval(CurrentWave);

            Debug.Log($"Starting Wave {CurrentWave} | Budget: {currentWaveBudget} | Interval: {currentSpawnInterval}s");
            SetUpNewWave();
        }
    }

    private float CalculateWaveBudget(int waveNumber)
    {
        // Formula: (Base * (Growth ^ Wave)) + (Step * Wave)
        return (baseBudget * Mathf.Pow(budgetGrowthMultiplier, waveNumber - 1)) + (budgetStep * (waveNumber - 1));
    }

    private float CalculateSpawnInterval(int waveNumber)
    {
        // Multiplies the interval by a decay rate each wave, clamping to a safe minimum
        float calculatedInterval = baseSpawnInterval * Mathf.Pow(intervalDecayRate, waveNumber - 1);
        return Mathf.Max(calculatedInterval, minSpawnInterval);
    }

    private void Update()
    {
        if (!waveInProgress) return;

        if (!spawingInProgess && activeEnemiesAmount <= 0)
        {
            EndCurrentWave();
        }

        if (currentWaveSpawnQueue.Count <= 0 && spawingInProgess)
        {
            spawingInProgess = false;
        }
        else if(spawingInProgess && currentWaveSpawnQueue.Count > 0)
        {
            SpawnEnemyEntries();
        }
    }

    private void SpawnEnemyEntries()
    {
        if (Time.time > lastSpawnTime + currentSpawnInterval)
        {
            lastSpawnTime = Time.time;

            SpawnEntry entry = currentWaveSpawnQueue.Dequeue();

            for (int i = 0; i < entry.AmountToSpawn; i++)
            {
                activeEnemiesAmount++;

                EnemyController instance = null;
                if (EnemyPool.Instance != null)
                {
                    instance = EnemyPool.Instance.GetEnemy(entry.EnemyData.EnemyPrefab, GetRandomSpawnPosition(), 
                                                           Quaternion.identity);
                }
                else instance = Instantiate(entry.EnemyData.EnemyPrefab, GetRandomSpawnPosition(),
                                            Quaternion.identity);

                instance.OnRemoveEnemyFromWave = EnemyKilled;
                instance.gameObject.SetActive(true);
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPoint = this.spawnPoint.position;

        spawnPoint.x += Random.Range(-offsetRange.x, 0);
        spawnPoint.y += Random.Range(-offsetRange.y, offsetRange.y);

        //Debug.Log("Enemy Spawn at:" + spawnPoint);

        return spawnPoint;
    }

    private void SetUpNewWave()
    {
        allEnemiesInWaveDead = false;
        currentWaveSpawnQueue.Clear();

        if (wavesBlueprint && wavesBlueprint.EnemyWaves.ContainsKey(CurrentWave))
        {
            for (int i = 0; i < wavesBlueprint.EnemyWaves[CurrentWave].Count; i++)
            {
                currentWaveSpawnQueue.Enqueue(wavesBlueprint.EnemyWaves[CurrentWave][i]);
            }
        }
        else
        {
            CreateProceduralWave();
        }

        spawingInProgess = true;
        waveInProgress = true;
    }

    public void EndCurrentWave()
    {
        Debug.Log($"Wave {CurrentWave} Defeated.");

        waveInProgress = false;
        allEnemiesInWaveDead = true;
    }

    private void CreateProceduralWave()
    {
        while(currentWaveBudget > 0)
        {
            SO_EnemyData chosenEnemy = SelectAffordableEnemy(currentWaveBudget);

            if (chosenEnemy == null)
            {
                currentWaveBudget = 0;
                continue;
            }

            int amount = chosenEnemy.AmountToSpawn;

            if (CurrentWave > chosenEnemy.WaveBurstShift)
            {
                int wavesPastShift = CurrentWave - chosenEnemy.WaveBurstShift;
                float dynamicAmount = wavesPastShift * chosenEnemy.BurstShiftPerWave;
                amount += Mathf.Clamp(Mathf.FloorToInt(dynamicAmount), chosenEnemy.MinBurstClamp, chosenEnemy.MaxBurstClamp);
            }

            SpawnEntry enemyEntry = new SpawnEntry(chosenEnemy, amount);

            currentWaveSpawnQueue.Enqueue(enemyEntry);
            currentWaveBudget -= chosenEnemy.SpawnCost;
        }
    }

    private SO_EnemyData SelectAffordableEnemy(float budget)
    {
        // 1. Filter by budget AND wave requirement
        List<SO_EnemyData> eligibleEnemies = availableEnemies.FindAll(e =>
            e.SpawnCost <= budget && CurrentWave >= e.MinWaveRequirement);

        if (eligibleEnemies.Count == 0) return null;

        // 2. Calculate dynamic weights and total weight pool
        float totalWeight = 0f;
        List<float> calculatedWeights = new List<float>();

        for (int i = 0; i < eligibleEnemies.Count; i++)
        {
            SO_EnemyData enemy = eligibleEnemies[i];
            float dynamicWeight = enemy.BaseWeight;

            // Apply shift if current wave has surpassed the enemy's peak wave
            if (CurrentWave > enemy.WaveWeightShift)
            {
                int wavesPastPeak = CurrentWave - enemy.WaveWeightShift;
                dynamicWeight += wavesPastPeak * enemy.WeightShiftPerWave;
                dynamicWeight = Mathf.Clamp(dynamicWeight, enemy.MinWeightClamp, enemy.MaxWeightClamp);
            }

            calculatedWeights.Add(dynamicWeight);
            totalWeight += dynamicWeight;
        }

        // Edge case: If all weights drop to 0, fallback to standard random pick
        if (totalWeight <= 0)
        {
            return eligibleEnemies[Random.Range(0, eligibleEnemies.Count)];
        }

        // 3. Roll a value within the dynamic weight total
        float roll = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        // 4. Select the matching enemy
        for (int i = 0; i < eligibleEnemies.Count; i++)
        {
            cumulativeWeight += calculatedWeights[i];
            if (roll <= cumulativeWeight)
            {
                return eligibleEnemies[i];
            }
        }

        return eligibleEnemies[eligibleEnemies.Count - 1];
    }

    public void EnemyKilled()
    {
        activeEnemiesAmount--;
    }
}
