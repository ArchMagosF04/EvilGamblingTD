using Alchemy.Inspector;
using UnityEngine;

public class TowerDetectionRange : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private SO_TowerData towerData;
     private Animator animator;

    [BoxGroup("Debug"), SerializeField] private bool ShowGizmos;

    private bool enemyDetected;
    private float attackTimer;

    public static readonly int actionAnim = Animator.StringToHash("Attack");

    public void InitializedDetectionRange(SO_TowerData data, Animator anim)
    {
        towerData = data;
        animator = anim;
    }

    public void UpdateDetection()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGamePaused) return;

        attackTimer += Time.deltaTime * GameManager.Instance.GameSpeed;
        
        if (attackTimer > towerData.AttackSpeed) CheckDetection();
    }

    private void CheckDetection()
    {
        switch (towerData.RangeType)
        {
            case TowerRangeType.ForwardLine:
                ForwardLineDetection();
                break;
            case TowerRangeType.VerticalLine:
                VerticalLineDetection();
                break;
            case TowerRangeType.Radius:
                RadiusDetection();
                break;
            case TowerRangeType.ForwardCone:
                ForwardConeDetection();
                break;
            case TowerRangeType.VerticalCone:
                VerticalConeDetection();
                break;
        }
    }

    private void ForwardLineDetection()
    {
        if (Physics.SphereCast(transform.position + towerData.DetectionOriginOffset, towerData.DetectionRadius, -Vector3.right, out RaycastHit hit, towerData.DetectionRange, towerData.EnemyLayer))
        {
            enemyDetected = true;

            animator.SetTrigger(actionAnim);

            attackTimer = 0;

            AttackObject instance = null;
            if (AttackPool.Instance != null)
            {
                instance = AttackPool.Instance.GetAttack(towerData.AttackPrefab, transform.position,
                                                       Quaternion.identity);
            }
            else instance = Instantiate(towerData.AttackPrefab, transform.position,
                                                       Quaternion.identity);

            instance.gameObject.SetActive(true);
            instance.InitializeProjectile(Vector3.left);
        }
        else
        {
            enemyDetected = false;
        }
    }

    private void VerticalLineDetection()
    {

    }

    private void RadiusDetection()
    {

    }

    private void ForwardConeDetection()
    {

    }

    private void VerticalConeDetection()
    {

    }

    private void OnDrawGizmos()
    {
        if (!ShowGizmos) return;

        switch (towerData.RangeType)
        {
            case TowerRangeType.ForwardLine:
                GizmosForwardLine();
                break;
            case TowerRangeType.VerticalLine:
                GizmosVerticalLine();
                break;
            case TowerRangeType.Radius:
                GizmosRadius();
                break;
            case TowerRangeType.ForwardCone:
                GizmosForwardCone();
                break;
            case TowerRangeType.VerticalCone:
                GizmosVerticalCone();
                break;
        }
    }

    private void GizmosForwardLine()
    {
        
        if (enemyDetected)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Vector3 origin = transform.position + towerData.DetectionOriginOffset;

        Gizmos.DrawWireSphere(origin, towerData.DetectionRadius);

        Vector3 endPosition = (origin) - (transform.right * towerData.DetectionRange);
        Gizmos.DrawWireSphere(endPosition, towerData.DetectionRadius);

        Gizmos.DrawLine(origin + transform.forward * towerData.DetectionRadius, endPosition + transform.forward * towerData.DetectionRadius);
        Gizmos.DrawLine(origin - transform.forward * towerData.DetectionRadius, endPosition - transform.forward * towerData.DetectionRadius);
        Gizmos.DrawLine(origin + transform.up * towerData.DetectionRadius, endPosition + transform.up * towerData.DetectionRadius);
        Gizmos.DrawLine(origin - transform.up * towerData.DetectionRadius, endPosition - transform.up * towerData.DetectionRadius);
    }

    private void GizmosVerticalLine()
    {

    }

    private void GizmosRadius()
    {

    }

    private void GizmosForwardCone()
    {

    }

    private void GizmosVerticalCone()
    {

    }
}
