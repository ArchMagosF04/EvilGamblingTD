using Alchemy.Inspector;
using UnityEngine;

public class TowerDetectionRange : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private SO_TowerData towerData;
     private Animator animator;

    [BoxGroup("Range Visual"), SerializeField] private SpriteRenderer lineVisual;
    [BoxGroup("Range Visual"), SerializeField] private SpriteRenderer radiusVisual;
    [BoxGroup("Range Visual"), SerializeField] private SpriteRenderer coneVisual;

    [BoxGroup("Debug"), SerializeField] private bool ShowGizmos;

    private bool enemyDetected;
    private float attackTimer;

    public static readonly int actionAnim = Animator.StringToHash("Attack");

    private void Awake()
    {
        lineVisual.enabled = false;
        radiusVisual.enabled = false;
        coneVisual.enabled = false;
    }

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
            //case TowerRangeType.VerticalCone:
            //    VerticalConeDetection();
            //    break;
        }
    }

    #region Range Indicators

    [Button, BoxGroup("Range Visual")]
    public void ToggleRangeIndicator(bool input)
    {
        switch (towerData.RangeType)
        {
            case TowerRangeType.ForwardLine:
                lineVisual.enabled = input;
                break;
            case TowerRangeType.VerticalLine:
                lineVisual.enabled = input;
                break;
            case TowerRangeType.Radius:
                radiusVisual.enabled = input;
                break;
            case TowerRangeType.ForwardCone:
                coneVisual.enabled = input;
                break;
            //case TowerRangeType.VerticalCone:

            //    break;
        }
    }

    [Button, BoxGroup("Range Visual")]
    public void CreateRangeIndicator()
    {
        switch (towerData.RangeType)
        {
            case TowerRangeType.ForwardLine:
                ForwardLineIndicator();
                break;
            case TowerRangeType.VerticalLine:
                VerticalLineIndicator();
                break;
            case TowerRangeType.Radius:
                RadiusIndicator();
                break;
            case TowerRangeType.ForwardCone:
                ConeIndicator();
                break;
            //case TowerRangeType.VerticalCone:

            //    break;
        }
    }

    private void ForwardLineIndicator()
    {
        lineVisual.transform.localScale = new Vector3(towerData.DetectionRange + (towerData.DetectionRadius * 2), towerData.DetectionRadius * 2f, 1);
        lineVisual.transform.localPosition = new Vector3(-towerData.DetectionRange * 0.5f, 0, 0) + towerData.DetectionOriginOffset;
    }

    private void VerticalLineIndicator()
    {
        lineVisual.transform.localScale = new Vector3(towerData.DetectionRadius * 2f, (towerData.DetectionRange + towerData.DetectionRadius) * 2f, 1);
        lineVisual.transform.localPosition = Vector3.zero + towerData.DetectionOriginOffset;
    }

    private void RadiusIndicator()
    {

    }

    private void ConeIndicator()
    {

    }

    #endregion

    #region Detection

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
            instance.InitializeProjectile(Vector3.left, towerData.DetectionRange);
        }
        else
        {
            enemyDetected = false;
        }
    }

    private void VerticalLineDetection()
    {
        RaycastHit upperHit, lowerhit;

        if (Physics.SphereCast(transform.position + towerData.DetectionOriginOffset, towerData.DetectionRadius, Vector3.up, 
                               out upperHit, towerData.DetectionRange, towerData.EnemyLayer))
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
            instance.InitializeProjectile(Vector3.up, towerData.DetectionRange);

            AttackObject instance2 = null;
            if (AttackPool.Instance != null)
            {
                instance2 = AttackPool.Instance.GetAttack(towerData.AttackPrefab, transform.position,
                                                       Quaternion.identity);
            }
            else instance2 = Instantiate(towerData.AttackPrefab, transform.position,
                                                       Quaternion.identity);

            instance2.gameObject.SetActive(true);
            instance2.InitializeProjectile(Vector3.down, towerData.DetectionRange);
        }
        else if (Physics.SphereCast(transform.position + towerData.DetectionOriginOffset, towerData.DetectionRadius, Vector3.down,
                               out lowerhit, towerData.DetectionRange, towerData.EnemyLayer))
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
            instance.InitializeProjectile(Vector3.up, towerData.DetectionRange);

            AttackObject instance2 = null;
            if (AttackPool.Instance != null)
            {
                instance2 = AttackPool.Instance.GetAttack(towerData.AttackPrefab, transform.position,
                                                       Quaternion.identity);
            }
            else instance2 = Instantiate(towerData.AttackPrefab, transform.position,
                                                       Quaternion.identity);

            instance2.gameObject.SetActive(true);
            instance2.InitializeProjectile(Vector3.down, towerData.DetectionRange);
        }
        else
        {
            enemyDetected = false;
        }
    }

    private void RadiusDetection()
    {

    }

    private void ForwardConeDetection()
    {

    }

    #endregion

    #region Gizmos

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
            //case TowerRangeType.VerticalCone:
            //    GizmosVerticalCone();
            //    break;
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
        if (enemyDetected)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Vector3 origin = transform.position + towerData.DetectionOriginOffset + new Vector3(0, towerData.DetectionRange, 0);

        Gizmos.DrawWireSphere(origin, towerData.DetectionRadius);

        Vector3 endPosition = (origin) - (transform.up * towerData.DetectionRange * 2);
        Gizmos.DrawWireSphere(endPosition, towerData.DetectionRadius);

        Gizmos.DrawLine(origin + transform.forward * towerData.DetectionRadius, endPosition + transform.forward * towerData.DetectionRadius);
        Gizmos.DrawLine(origin - transform.forward * towerData.DetectionRadius, endPosition - transform.forward * towerData.DetectionRadius);
        Gizmos.DrawLine(origin + transform.right * towerData.DetectionRadius, endPosition + transform.right * towerData.DetectionRadius);
        Gizmos.DrawLine(origin - transform.right * towerData.DetectionRadius, endPosition - transform.right * towerData.DetectionRadius);
    }

    private void GizmosRadius()
    {

    }

    private void GizmosForwardCone()
    {

    }

    #endregion
}
