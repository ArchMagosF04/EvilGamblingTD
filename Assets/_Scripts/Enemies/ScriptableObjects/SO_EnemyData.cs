using Alchemy.Inspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Scriptable Objects/Enemies/Enemy Data")]
public class SO_EnemyData : ScriptableObject
{
    [field: SerializeField] public int ID { get; private set; }

    #region Spawn Settings

    [field: TabGroup("Spawn Settings", "Spawn Main"), SerializeField, AssetsOnly] public EnemyController EnemyPrefab;
    [field: TabGroup("Spawn Settings", "Spawn Main"), SerializeField] public float SpawnCost { get; private set; } = 1f;
    [field: TabGroup("Spawn Settings", "Spawn Main"), SerializeField, Tooltip("The wave number where this enemy starts appearing")] public int MinWaveRequirement { get; private set; } = 1;

    [field: TabGroup("Spawn Settings", "Spawn Weight"), SerializeField, Tooltip("Base frequency multiplier at wave 1")] public float BaseWeight { get; private set; } = 100f;
    [field: TabGroup("Spawn Settings", "Spawn Weight"), SerializeField, Tooltip("The wave number where this enemy peaks or starts fading")] public int WaveWeightShift { get; private set; } = 5;
    [field: TabGroup("Spawn Settings", "Spawn Weight"), SerializeField, Tooltip("How much weight shifts per wave after the Wave Shift (Negative to fade out, Positive to ramp up)")] public float WeightShiftPerWave { get; private set; } = -5f;
    [field: TabGroup("Spawn Settings", "Spawn Weight"), SerializeField, Tooltip("The absolute lowest weight this enemy can drop to")] public float MinWeightClamp { get; private set; } = 0f;
    [field: TabGroup("Spawn Settings", "Spawn Weight"), SerializeField, Tooltip("The absolute highest weight this enemy can climb to")] public float MaxWeightClamp { get; private set; } = 0f;

    [field: TabGroup("Spawn Settings", "Burst Spawn"), SerializeField, Tooltip("When this enemy is chosen to be spawned it will spawn this many number of instances")] public int AmountToSpawn { get; private set; } = 1;
    [field: TabGroup("Spawn Settings", "Burst Spawn"), SerializeField, Tooltip("The wave number where this enemy's spawn amount starts to change")] public int WaveBurstShift { get; private set; } = 10;
    [field: TabGroup("Spawn Settings", "Burst Spawn"), SerializeField, Tooltip("How much the amount spawned shifts after the Wave Shift")] public float BurstShiftPerWave { get; private set; } = 0.2f;
    [field: TabGroup("Spawn Settings", "Burst Spawn"), SerializeField, Tooltip("The absolute lowest amount of instances this enemy can spawn in one burst"), Min(1)] public int MinBurstClamp { get; private set; } = 1;
    [field: TabGroup("Spawn Settings", "Burst Spawn"), SerializeField, Tooltip("The absolute highest amount of instances this enemy can spawn in one burst")] public int MaxBurstClamp { get; private set; } = 15;

    #endregion

    #region Enemy Stats

    [field: BoxGroup("Enemy Stats"), SerializeField] public float MoveSpeed { get; private set; } = 5f;
    [field: BoxGroup("Enemy Stats"), SerializeField] public float MaxHealth { get; private set; } = 1f;
    [field: BoxGroup("Enemy Stats"), SerializeField] public float BaseDamage { get; private set; } = 1f;
    #endregion

    #region Enemy Attack
    [field: TabGroup("Attack Settings", "Attack Stats"), SerializeField] public float AttackDamage { get; private set; } = 1f;
    [field: TabGroup("Attack Settings", "Attack Stats"), SerializeField] public float AttackSpeed { get; private set; } = 1.5f;
    [field: TabGroup("Attack Settings", "Attack Range"), SerializeField] public float AttackRadius { get; private set; } = 1f;
    [field: TabGroup("Attack Settings", "Attack Range"), SerializeField] public float AttackRange { get; private set; } = 0.5f;

    #endregion

    #region Detection/Attack Range

    [field: BoxGroup("Detection Range"), SerializeField] public LayerMask DetectionMask { get; private set; }
    [field: BoxGroup("Detection Range"), SerializeField] public float DetectionRadius { get; private set; } = 1f;
    [field: BoxGroup("Detection Range"), SerializeField] public float DetectionRange { get; private set; } = 0.5f;
    [field: BoxGroup("Detection Range"), SerializeField] public float DetectionTickTime { get; private set; } = 0.5f;
    

    #endregion

    [Button]
    public void GiveEnemyDataToPrefab()
    {
        if (EnemyPrefab == null)
        {
            Debug.LogWarning("No tower prefab found.");
            return;
        }

        EnemyPrefab.ReceiveEnemyData(this);
    }
}
