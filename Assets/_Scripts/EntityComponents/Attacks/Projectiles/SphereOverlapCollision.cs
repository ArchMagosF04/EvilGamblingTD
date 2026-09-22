using UnityEngine;

public class SphereOverlapCollision : MonoBehaviour, IProjectileCollider
{
    [SerializeField] private SO_Attack attackData;

    [SerializeField] private bool DebugHitbox;

    public RaycastHit[] GetMultipleCollisions()
    {
        throw new System.NotImplementedException();
    }

    public RaycastHit GetSingleCollision()
    {
        throw new System.NotImplementedException();
    }

    public void GiveAttackData(SO_Attack data)
    {
        attackData = data;
    }

    private void OnDrawGizmos()
    {
        if (!DebugHitbox) return;

        Gizmos.DrawWireSphere(transform.position, attackData.Radius);
    }
}
