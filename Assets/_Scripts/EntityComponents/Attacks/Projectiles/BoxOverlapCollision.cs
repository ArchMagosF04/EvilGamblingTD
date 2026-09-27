using UnityEngine;

public class BoxOverlapCollision : MonoBehaviour, IProjectileCollider
{
    [SerializeField] private SO_Attack attackData;

    [SerializeField] private bool DebugHitbox;

    private Vector3 detectionCubeCenter;
    private Vector3 detectionCubeSize;

    public void GiveAttackData(SO_Attack data)
    {
        attackData = data;
    }

    //public Collider[] GetMultipleCollisions()
    //{
    //    detectionCubeCenter = new Vector3(transform.position.x + attackData.OriginOffset.x + (attackData.Size.x / 2),
    //        transform.position.y + attackData.OriginOffset.y + (attackData.Size.y / 2), transform.position.z + attackData.OriginOffset.z);

    //    detectionCubeSize = new Vector3(attackData.Size.x, attackData.Size.y, attackData.Size.z);

    //    return Physics.OverlapBox(detectionCubeCenter, detectionCubeSize / 2, Quaternion.LookRotation(transform.right, transform.up), attackData.TargetMask);
    //}

    //public Collider GetSingleCollision()
    //{
    //    detectionCubeCenter = new Vector3(transform.position.x + attackData.OriginOffset.x + (attackData.Size.x / 2),
    //        transform.position.y + attackData.OriginOffset.y + (attackData.Size.y / 2), transform.position.z + attackData.OriginOffset.z);

    //    detectionCubeSize = new Vector3(attackData.Size.x, attackData.Size.y, attackData.Size.z);

    //    Collider[] cols = Physics.OverlapBox(detectionCubeCenter, detectionCubeSize / 2, Quaternion.LookRotation(transform.right, transform.up), attackData.TargetMask);

    //    foreach (Collider col in cols)
    //    {
    //        if (col != null) return col;
    //    }

    //    return null;
    //}

    //private void OnDrawGizmos()
    //{
    //    if (!DebugHitbox) return;

    //    Matrix4x4 oldMatrix = Gizmos.matrix;

    //    Quaternion rotation = Quaternion.LookRotation(transform.right, transform.up);

    //    //Gizmos.matrix = Matrix4x4.Rotate(rotation);

    //    Vector3 cubeSize = new Vector3(attackData.Size.x, attackData.Size.y, attackData.Size.z);

    //    Vector3 cubeCenter = new Vector3(transform.position.x + attackData.OriginOffset.x + (attackData.Size.x / 2),
    //        transform.position.y + attackData.OriginOffset.y + (attackData.Size.y / 2), transform.position.z + attackData.OriginOffset.z);

    //    if (Physics.OverlapBox(cubeCenter, cubeSize / 2, rotation, attackData.TargetMask).Length > 0)
    //    {
    //        Gizmos.color = Color.red;
    //    }
    //    else Gizmos.color = Color.green;


    //    Gizmos.DrawWireCube(cubeCenter, cubeSize);

    //    Gizmos.matrix = oldMatrix;
    //}
}
