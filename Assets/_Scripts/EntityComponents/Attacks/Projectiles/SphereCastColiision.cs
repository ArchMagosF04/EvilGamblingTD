using UnityEngine;

public class SphereCastColiision : MonoBehaviour, IProjectileCollider
{
    [SerializeField] private SO_Attack attackData;

    [SerializeField] private bool DebugHitbox;

    public Collider[] GetMultipleCollisions()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position + attackData.OriginOffset, attackData.Radius, transform.right, attackData.Range, attackData.TargetMask);

        Collider[] cols = new Collider[hits.Length];

        for (int i = 0; i < hits.Length; i++)
        {
            cols[i] = hits[i].collider;
        }

        return cols;
    }

    public Collider GetSingleCollision()
    {
        RaycastHit hit;

        Physics.SphereCast(transform.position + attackData.OriginOffset, attackData.Radius, transform.right, out hit, attackData.Range, attackData.TargetMask);

        return hit.collider;
    }

    public void GiveAttackData(SO_Attack data)
    {
        attackData = data;
    }

    private void OnDrawGizmos()
    {
        if (!DebugHitbox) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + attackData.OriginOffset, attackData.Radius);

        Vector3 endPosition = (transform.position + attackData.OriginOffset) + (transform.right * attackData.Range);
        Gizmos.DrawWireSphere(endPosition, attackData.Radius);

        Gizmos.DrawLine((transform.position + attackData.OriginOffset) + transform.forward * attackData.Radius, endPosition + transform.forward * attackData.Radius);
        Gizmos.DrawLine((transform.position + attackData.OriginOffset) - transform.forward * attackData.Radius, endPosition - transform.forward * attackData.Radius);
        Gizmos.DrawLine((transform.position + attackData.OriginOffset) + transform.up * attackData.Radius, endPosition + transform.up * attackData.Radius);
        Gizmos.DrawLine((transform.position + attackData.OriginOffset) - transform.up * attackData.Radius, endPosition - transform.up * attackData.Radius);
    }
}
