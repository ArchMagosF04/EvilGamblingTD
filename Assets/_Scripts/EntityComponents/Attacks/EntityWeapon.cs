using UnityEngine;
using Alchemy.Inspector;

public class EntityWeapon : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private SO_Attack attackData;

    public void InitializedWeapon()
    {

    }

    public void ExecuteAttack(Vector3 direction, float speed = 0)
    {

    }
}
