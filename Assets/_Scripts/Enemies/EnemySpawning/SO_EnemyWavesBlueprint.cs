using UnityEngine;
using Alchemy.Serialization;
using System;
using System.Collections.Generic;
using Alchemy.Inspector;

[CreateAssetMenu(fileName = "New EnemyWavesBlueprint", menuName = "Scriptable Objects/Enemies/Enemy Waves Blueprint"), AlchemySerialize]
public partial class SO_EnemyWavesBlueprint : ScriptableObject
{
    [HelpBox("When editing the dictionary change the value of this number so the changes are saved.")]
    [SerializeField, Range(0f,1f)] private float change;
    [AlchemySerializeField, NonSerialized] public Dictionary<int, List<SpawnEntry>> EnemyWaves = new();
}
