using Alchemy.Inspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Objects/Attack")]
public class SO_Attack : ScriptableObject
{
    [field: SerializeField] public string ID {  get; private set; }

    [field: BoxGroup("Main Stats"), SerializeField] public float Damage { get; private set; }
    [field: BoxGroup("Main Stats"), SerializeField, Min(0)] public float Pierce { get; private set; }
    [field: BoxGroup("Main Stats"), SerializeField] public bool CanHitSameEnemyMultipleTimes { get; private set; }
    [field: BoxGroup("Main Stats"), SerializeField] public bool DestroyOnLastHit { get; private set; } = true;
    [field: BoxGroup("Main Stats"), SerializeField] public float MoveSpeed { get; private set; }
    [field: BoxGroup("Main Stats"), SerializeField] public float LifeTime { get; private set; } = 0.2f;
    [field: BoxGroup("Main Stats"), SerializeField] public bool DestroyBeyondRange { get; private set; } = true;

    [field: BoxGroup("Double Check Collision"), SerializeField] public bool CheckPreviousFramesCollision { get; private set; } = false;
    [field: BoxGroup("Double Check Collision"), SerializeField, ShowIf("CheckPreviousFramesCollision")] public LayerMask TargetMask { get; private set; }
    [field: BoxGroup("Double Check Collision"), SerializeField, ShowIf("CheckPreviousFramesCollision")] public Vector3 OriginOffset { get; private set; }
    [field: BoxGroup("Double Check Collision"), SerializeField, ShowIf("CheckPreviousFramesCollision")] public float Range { get; private set; }
    [field: BoxGroup("Double Check Collision"), SerializeField, ShowIf("CheckPreviousFramesCollision")] public float Radius { get; private set; }

}
