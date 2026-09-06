using Alchemy.Inspector;
using System;
using UnityEngine;

[Serializable]
public struct SpawnEntry
{
    public int AmountToSpawn;
    [AssetsOnly] public SO_EnemyData EnemyData;

    public SpawnEntry(SO_EnemyData data, int amount = 1)
    {
        EnemyData = data;
        AmountToSpawn = amount;
    }
}
