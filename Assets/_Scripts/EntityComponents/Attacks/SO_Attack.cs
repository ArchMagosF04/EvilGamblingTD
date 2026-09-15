using Alchemy.Inspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Data", menuName = "Scriptable Objects/Attack")]
public class SO_Attack : ScriptableObject
{
    [field: SerializeField] public string ID {  get; private set; }

    [field: BoxGroup("Main Stats"), SerializeField] public float Damage { get; private set; }
    [field: BoxGroup("Main Stats"), SerializeField] public float Pierce { get; private set; }
    [field: BoxGroup("Main Stats"), SerializeField] public float MoveSpeed { get; private set; }

    [field: BoxGroup("Hitbox"), SerializeField] public LayerMask TargetMask { get; private set; }
    [field: BoxGroup("Hitbox"), SerializeField] public bool CanHitMultipleTimes { get; private set; } = false;
    [field: BoxGroup("Hitbox"), SerializeField] public float Range { get; private set; }
    [field: BoxGroup("Hitbox"), SerializeField] public float Radius { get; private set; }
    [field: BoxGroup("Hitbox"), SerializeField] public Vector2 Size { get; private set; }

}
