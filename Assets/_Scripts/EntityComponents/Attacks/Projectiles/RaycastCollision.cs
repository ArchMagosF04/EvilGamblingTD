using UnityEngine;

public class RaycastCollision : MonoBehaviour, IProjectileCollider
{
    [SerializeField] private SO_Attack attackData;

    [SerializeField] private bool DebugHitbox;

    public Collider[] GetMultipleCollisions()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position + attackData.OriginOffset, transform.right, attackData.Range, attackData.TargetMask);

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

        Physics.Raycast(transform.position + attackData.OriginOffset, transform.right, out hit, attackData.Range, attackData.TargetMask);

        return hit.collider;
    }

    public void GiveAttackData(SO_Attack data)
    {
        attackData = data;
    }

    private void OnDrawGizmos()
    {
        if (!DebugHitbox) return;

        Gizmos.DrawLine(transform.position + attackData.OriginOffset, transform.position + transform.right * attackData.Range);
    }
}
