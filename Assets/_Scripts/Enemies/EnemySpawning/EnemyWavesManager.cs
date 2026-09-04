using Alchemy.Inspector;
using Alchemy.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWavesManager : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private Transform spawnPoint;
    [BoxGroup("Components"), SerializeField] private List<SO_EnemyData> availableEnemies;

    //SO for premade waves. [AlchemySerializeField, NonSerialized] public Dictionary<string, GameObject> dictionary = new();


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
    [BoxGroup("Spawn Timing Settings"), SerializeField, Tooltip("How much faster enemies spawn each wave")]
    private float intervalDecayRate = 0.95f;

    public int CurrentWave { get; private set; } = 0;
    private bool isSpawning = false;


    public void StartNextWave()
    {
        if (!isSpawning)
        {
            CurrentWave++;
            float waveBudget = CalculateWaveBudget(CurrentWave);
            float waveInterval = CalculateWaveInterval(CurrentWave);

            Debug.Log($"Starting Wave {CurrentWave} | Budget: {waveBudget} | Interval: {waveInterval}s");
            //StartCoroutine(SpawnInfiniteWaveRoutine(waveBudget, waveInterval));
        }
    }

    private float CalculateWaveBudget(int waveNumber)
    {
        // Formula: (Base * (Growth ^ Wave)) + (Step * Wave)
        return (baseBudget * Mathf.Pow(budgetGrowthMultiplier, waveNumber - 1)) + (budgetStep * (waveNumber - 1));
    }

    private float CalculateWaveInterval(int waveNumber)
    {
        // Multiplies the interval by a decay rate each wave, clamping to a safe minimum
        float calculatedInterval = baseSpawnInterval * Mathf.Pow(intervalDecayRate, waveNumber - 1);
        return Mathf.Max(calculatedInterval, minSpawnInterval);
    }

    //private IEnumerator SpawnInfiniteWaveRoutine(float currentBudget, float spawnInterval)
    //{
    //    isSpawning = true;
    //    float remainingBudget = currentBudget;

    //    while (remainingBudget > 0)
    //    {
    //        EnemyType chosenEnemy = SelectAffordableEnemy(remainingBudget);

    //        if (chosenEnemy != null)
    //        {
    //            Instantiate(chosenEnemy.prefab, spawnPoint.position, spawnPoint.rotation);
    //            remainingBudget -= chosenEnemy.cost;
    //        }
    //        else
    //        {
    //            // Break if no enemies are cheap enough for the remaining budget
    //            break;
    //        }

    //        yield return new WaitForSeconds(spawnInterval);
    //    }

    //    isSpawning = false;
    //    Debug.Log($"Wave {CurrentWave} spawning complete!");
    //}

    //private EnemyType SelectAffordableEnemy2(float budget)
    //{
    //    // 1. Filter the catalog for enemies we can actually afford
    //    List<EnemyType> affordable = availableEnemies.FindAll(e => e.cost <= budget);

    //    if (affordable.Count == 0) return null;

    //    // 2. Calculate the total combined weight of all affordable choices
    //    float totalWeight = 0f;
    //    foreach (EnemyType enemy in affordable)
    //    {
    //        totalWeight += enemy.weight;
    //    }

    //    // 3. Roll a random value between 0 and the total weight
    //    float roll = Random.Range(0f, totalWeight);
    //    float cumulativeWeight = 0f;

    //    // 4. Determine which enemy container the roll landed in
    //    foreach (EnemyType enemy in affordable)
    //    {
    //        cumulativeWeight += enemy.weight;
    //        if (roll <= cumulativeWeight)
    //        {
    //            return enemy;
    //        }
    //    }

    //    // Fallback security check
    //    return affordable[affordable.Count - 1];
    //}

    //private EnemyType SelectAffordableEnemy3(float budget)
    //{
    //    // 1. Filter by budget AND wave requirement
    //    List<EnemyType> eligibleEnemies = availableEnemies.FindAll(e =>
    //        e.cost <= budget && CurrentWave >= e.minWaveRequirement);

    //    if (eligibleEnemies.Count == 0) return null;

    //    // 2. Calculate dynamic weights and total weight pool
    //    float totalWeight = 0f;
    //    List<float> calculatedWeights = new List<float>();

    //    for (int i = 0; i < eligibleEnemies.Count; i++)
    //    {
    //        EnemyType enemy = eligibleEnemies[i];
    //        float dynamicWeight = enemy.baseWeight;

    //        // Apply shift if current wave has surpassed the enemy's peak wave
    //        if (CurrentWave > enemy.peakWave)
    //        {
    //            int wavesPastPeak = CurrentWave - enemy.peakWave;
    //            dynamicWeight += wavesPastPeak * enemy.weightShiftPerWave;
    //            dynamicWeight = Mathf.Max(dynamicWeight, enemy.minWeightClamp);
    //        }

    //        calculatedWeights.Add(dynamicWeight);
    //        totalWeight += dynamicWeight;
    //    }

    //    // Edge case: If all weights drop to 0, fallback to standard random pick
    //    if (totalWeight <= 0)
    //    {
    //        return eligibleEnemies[Random.Range(0, eligibleEnemies.Count)];
    //    }

    //    // 3. Roll a value within the dynamic weight total
    //    float roll = Random.Range(0f, totalWeight);
    //    float cumulativeWeight = 0f;

    //    // 4. Select the matching enemy
    //    for (int i = 0; i < eligibleEnemies.Count; i++)
    //    {
    //        cumulativeWeight += calculatedWeights[i];
    //        if (roll <= cumulativeWeight)
    //        {
    //            return eligibleEnemies[i];
    //        }
    //    }

    //    return eligibleEnemies[eligibleEnemies.Count - 1];
    //}
}
