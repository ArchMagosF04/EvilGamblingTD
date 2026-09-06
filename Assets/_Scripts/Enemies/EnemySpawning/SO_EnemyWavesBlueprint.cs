using UnityEngine;
using Alchemy.Serialization;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New EnemyWavesBlueprint", menuName = "Scriptable Objects/Enemies/Enemy Waves Blueprint"), AlchemySerialize]
public partial class SO_EnemyWavesBlueprint : ScriptableObject
{
    [AlchemySerializeField, NonSerialized] public Dictionary<int, List<SpawnEntry>> EnemyWaves;
}
