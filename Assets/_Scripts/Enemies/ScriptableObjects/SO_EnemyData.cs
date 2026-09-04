using Alchemy.Inspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Scriptable Objects/Enemies/Enemy Data")]
public class SO_EnemyData : ScriptableObject
{
    [field: SerializeField] public int ID { get; private set; }

    #region Spawn Settings

    [field: BoxGroup("Spawn Settings"), SerializeField, AssetsOnly] public EnemyController EnemyPrefab;
    [field: BoxGroup("Spawn Settings"), SerializeField] public float SpawnCost { get; private set; } = 1f;

    [field: BoxGroup("Spawn Settings"), SerializeField, Tooltip("Base frequency multiplier at wave 1")] public float BaseWeight { get; private set; } = 100f;
    [field: BoxGroup("Spawn Settings"), SerializeField, Tooltip("The wave number where this enemy starts appearing")] public int MinWaveRequirement { get; private set; } = 1;
    [field: BoxGroup("Spawn Settings"), SerializeField, Tooltip("The wave number where this enemy peaks or starts fading")] public int PeakWave { get; private set; } = 5;
    [field: BoxGroup("Spawn Settings"), SerializeField, Tooltip("How much weight shifts per wave after the peak wave (Negative to fade out, Positive to ramp up)")] public float WeightShiftPerWave { get; private set; } = -5f;
    [field: BoxGroup("Spawn Settings"), SerializeField, Tooltip("The absolute lowest weight this enemy can drop to")] public float MinWeightClamp { get; private set; } = 0f;
    [field: BoxGroup("Spawn Settings"), SerializeField, Tooltip("The absolute highest weight this enemy can drop to")] public float MaxWeightClamp { get; private set; } = 0f;

    #endregion

    #region Enemy Stats

    [field: BoxGroup("Enemy Stats"), SerializeField] public float MoveSpeed { get; private set; } = 5f;
    [field: BoxGroup("Enemy Stats"), SerializeField] public float MaxHealth { get; private set; } = 1f;
    [field: BoxGroup("Enemy Stats"), SerializeField] public float Damage { get; private set; } = 1f;

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
