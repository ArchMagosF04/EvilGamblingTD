using UnityEngine;

public class BoxCastCollision : MonoBehaviour, IProjectileCollider
{
    private SO_Attack attackData;

    [SerializeField] private bool DebugHitbox;

    private Vector3 detectionCubeCenter;
    private Vector3 detectionCubeSize;

    public void GiveAttackData(SO_Attack data)
    {
        attackData = data;
    }

    public RaycastHit[] GetMultipleCollisions()
    {
        detectionCubeCenter = new Vector3(transform.position.x + (attackData.Size.x / 2),
            transform.position.y + (attackData.Size.y / 2), transform.position.z);

        detectionCubeSize = new Vector3(attackData.Size.x, attackData.Size.y, 0.2f);

        return Physics.BoxCastAll(detectionCubeCenter, detectionCubeSize / 2, transform.right, Quaternion.identity, attackData.Size.x, attackData.TargetMask);
    }

    public RaycastHit GetSingleCollision()
    {
        detectionCubeCenter = new Vector3(transform.position.x + (attackData.Size.x / 2),
            transform.position.y + (attackData.Size.y / 2), transform.position.z);

        detectionCubeSize = new Vector3(attackData.Size.x, attackData.Size.y, 0.2f);

        if (Physics.BoxCast(detectionCubeCenter, detectionCubeSize / 2, transform.right, out RaycastHit info, Quaternion.identity, attackData.Size.x, attackData.TargetMask))
        {
            return info;
        }
        else return new RaycastHit();
    }

    private void OnDrawGizmos()
    {
        if (!DebugHitbox) return;

        Vector3 cubeSize = new Vector3(attackData.Size.x, attackData.Size.y, 0.2f);

        DebugBoxCast.SimpleDrawBoxCast(transform.position, cubeSize / 2, Quaternion.identity, transform.right, attackData.Size.x, Color.cyan);
    }
}
