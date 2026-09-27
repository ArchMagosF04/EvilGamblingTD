using UnityEngine;

public class SphereOverlapCollision : MonoBehaviour, IProjectileCollider
{
    [SerializeField] private SO_Attack attackData;

    [SerializeField] private bool DebugHitbox;

    public Collider[] GetMultipleCollisions()
    {
        return Physics.OverlapSphere(transform.position + attackData.OriginOffset, attackData.Radius, attackData.TargetMask);
    }

    public Collider GetSingleCollision()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + attackData.OriginOffset, attackData.Radius, attackData.TargetMask);

        Debug.Log(cols.Length);

        foreach (Collider col in cols)
        {
            if (col != null) return col;
        }

        return null;
    }

    public void GiveAttackData(SO_Attack data)
    {
        attackData = data;
    }

    private void OnDrawGizmos()
    {
        if (!DebugHitbox) return;

        if (Physics.OverlapSphere(transform.position + attackData.OriginOffset, attackData.Radius, attackData.TargetMask).Length > 0)
        {
            Gizmos.color = Color.yellow;
        }
        else Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position + attackData.OriginOffset, attackData.Radius);
    }
}
