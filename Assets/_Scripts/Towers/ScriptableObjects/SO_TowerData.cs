using Alchemy.Inspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower Data", menuName = "Scriptable Objects/Towers/Tower Data")]
public class SO_TowerData : ScriptableObject
{
    [field: SerializeField] private int ID;

    #region Buy Settings

    [field: BoxGroup("Buy Settings"), SerializeField, AssetsOnly] public TowerController TowerPrefab {  get; private set; }
    [field: BoxGroup("Buy Settings"), SerializeField] public int TowerCost { get; private set; }
    [field: BoxGroup("Buy Settings"), SerializeField, AssetsOnly] public Sprite TowerButtonSprite { get; private set; }

    #endregion

    #region Tower Stats

    [field: BoxGroup("Tower Stats"), SerializeField] private float damage;

    #endregion
}
