using UnityEngine;

public class BoxCastCollision : MonoBehaviour, IProjectileCollider
{
    [SerializeField] private SO_Attack attackData;

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

        detectionCubeSize = new Vector3(attackData.Size.x, attackData.Size.y, attackData.Size.z);

        return Physics.BoxCastAll(detectionCubeCenter, detectionCubeSize / 2, transform.right, Quaternion.LookRotation(transform.right, transform.up), attackData.Range, attackData.TargetMask);
    }

    public RaycastHit GetSingleCollision()
    {
        detectionCubeCenter = new Vector3(transform.position.x + (attackData.Size.x / 2),
            transform.position.y + (attackData.Size.y / 2), transform.position.z);

        detectionCubeSize = new Vector3(attackData.Size.x, attackData.Size.y, attackData.Size.z);

        if (Physics.BoxCast(detectionCubeCenter, detectionCubeSize / 2, transform.right, out RaycastHit info, Quaternion.LookRotation(transform.right, transform.up), attackData.Range, attackData.TargetMask))
        {
            return info;
        }
        else return new RaycastHit();
    }

    private void OnDrawGizmos()
    {
        if (!DebugHitbox) return;

        Vector3 cubeSize = new Vector3(attackData.Size.x, attackData.Size.y, attackData.Size.z);

        DebugBoxCast.SimpleDrawBoxCast(transform.position, cubeSize / 2, Quaternion.LookRotation(transform.right, transform.up), transform.right, attackData.Range, Color.cyan);
    }
}
