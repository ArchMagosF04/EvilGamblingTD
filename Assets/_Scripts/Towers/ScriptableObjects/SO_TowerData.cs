using Alchemy.Inspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower Data", menuName = "Scriptable Objects/Towers/Tower Data")]
public class SO_TowerData : ScriptableObject
{
    [field: SerializeField] public string ID {  get; private set; }

    #region Buy Settings

    [field: BoxGroup("Buy Settings"), SerializeField, AssetsOnly] public TowerController TowerPrefab {  get; private set; }
    [field: BoxGroup("Buy Settings"), SerializeField] public int TowerCost { get; private set; }
    [field: BoxGroup("Buy Settings"), SerializeField, AssetsOnly] public Sprite TowerButtonSprite { get; private set; }

    #endregion

    #region Tower Stats

    [field: BoxGroup("Attack Settings"), SerializeField, AssetsOnly] public Projectile AttackPrefab { get; private set; }
    [field: BoxGroup("Attack Settings"), SerializeField] public float AttackSpeed { get; private set; } = 1.5f;
    [field: BoxGroup("Tower Stats"), SerializeField] public float Damage { get; private set; } = 1f;
    [field: BoxGroup("Tower Stats"), SerializeField] public float MaxHealth { get; private set; } = 5f;

    #endregion

    #region Detection Range

    [field: BoxGroup("DetectionRange"), SerializeField] public TowerRangeType RangeType { get; private set; }
    [field: BoxGroup("DetectionRange"), SerializeField] public LayerMask EnemyLayer { get; private set; }
    [field: BoxGroup("DetectionRange"), SerializeField] public float DetectionRange { get; private set; }
    [field: BoxGroup("DetectionRange"), SerializeField] public float DetectionRadius { get; private set; }
    [field: BoxGroup("DetectionRange"), SerializeField, Range(0f, 180f)] public float DetectionAngle { get; private set; } = -1f;


    #endregion

    [Button]
    public void GiveTowerDataToPrefab()
    {
        if (TowerPrefab == null)
        {
            Debug.LogWarning("No tower prefab found.");
            return;
        }

        TowerPrefab.ReceiveTowerData(this);
    }
}

public enum TowerRangeType
{
    ForwardLine,
    VerticalLine,
    Radius,
    ForwardCone,
    VerticalCone,
}
