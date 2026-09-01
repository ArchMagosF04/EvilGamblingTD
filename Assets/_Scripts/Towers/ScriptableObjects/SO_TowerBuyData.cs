using Alchemy.Inspector;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerBuyData", menuName = "Scriptable Objects/Towers/Buy Data")]
public class SO_TowerBuyData : ScriptableObject
{
    [field: SerializeField, AssetsOnly] public TowerController TowerPrefab {  get; private set; }
    [field: SerializeField, AssetsOnly] public int TowerCost { get; private set; }
    [field: SerializeField, AssetsOnly] public Sprite TowerButtonSprite { get; private set; }
}
