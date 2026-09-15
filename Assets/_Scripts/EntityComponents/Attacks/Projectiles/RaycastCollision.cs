using UnityEngine;

public class RaycastCollision : MonoBehaviour, IProjectileCollider
{
    [SerializeField] private SO_Attack attackData;

    [SerializeField] private bool DebugHitbox;

    public RaycastHit[] GetMultipleCollisions()
    {
        return Physics.RaycastAll(transform.position, transform.right, attackData.Range, attackData.TargetMask);
    }

    public RaycastHit GetSingleCollision()
    {
        RaycastHit hit;

        Physics.Raycast(transform.position, transform.right, out hit, attackData.Range, attackData.TargetMask);

        return hit;
    }

    public void GiveAttackData(SO_Attack data)
    {
        attackData = data;
    }

    private void OnDrawGizmos()
    {
        if (!DebugHitbox) return;

        Gizmos.DrawLine(transform.position, transform.position + transform.right * attackData.Range);
    }
}
