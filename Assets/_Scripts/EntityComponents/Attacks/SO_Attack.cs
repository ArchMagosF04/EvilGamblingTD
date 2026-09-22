using Alchemy.Inspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Objects/Attack")]
public class SO_Attack : ScriptableObject
{
    [field: SerializeField] public string ID {  get; private set; }

    [field: BoxGroup("Main Stats"), SerializeField] public float Damage { get; private set; }
    [field: BoxGroup("Main Stats"), SerializeField] public float Pierce { get; private set; }
    [field: BoxGroup("Main Stats"), SerializeField] public bool HitsMultipleEnemies { get; private set; } = false;
    [field: BoxGroup("Main Stats"), SerializeField] public bool DestroyOnLastHit { get; private set; } = true;
    [field: BoxGroup("Main Stats"), SerializeField] public bool AttacksOnlyOnce { get; private set; } = false;
    [field: BoxGroup("Main Stats"), SerializeField] public float MoveSpeed { get; private set; }
    [field: BoxGroup("Main Stats"), SerializeField] public float LifeTime { get; private set; } = 0.2f;

    [field: BoxGroup("Hitbox"), SerializeField] public LayerMask TargetMask { get; private set; }
    [field: BoxGroup("Hitbox"), SerializeField] public Vector3 OriginOffset { get; private set; }
    [field: BoxGroup("Hitbox"), SerializeField] public bool CanHitMultipleTimes { get; private set; } = false;
    [field: BoxGroup("Hitbox"), SerializeField] public float Range { get; private set; }
    [field: BoxGroup("Hitbox"), SerializeField] public float Radius { get; private set; }
    [field: BoxGroup("Hitbox"), SerializeField] public Vector3 Size { get; private set; }

}
