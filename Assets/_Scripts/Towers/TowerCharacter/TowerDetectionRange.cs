using Alchemy.Inspector;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        radiusVisual.transform.localScale = new Vector3(towerData.DetectionRange * 2, towerData.DetectionRange * 2, 1);
        radiusVisual.transform.localPosition = Vector3.zero + towerData.DetectionOriginOffset;
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

            float angleStep = (towerData.NumberOfShots > 1) ? towerData.ShotSpread / (towerData.NumberOfShots - 1) : 0f;

            float startAngle = (-towerData.ShotSpread / 2f) + 180f;

            for (int i = 0; i < towerData.NumberOfShots; i++)
            {
                float currentAngle = startAngle + (angleStep * i);

                Quaternion bulletRotation = transform.rotation * Quaternion.Euler(0, 0, currentAngle);

                CreateAttack(bulletRotation);
            }
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
        }
        else if (Physics.SphereCast(transform.position + towerData.DetectionOriginOffset, towerData.DetectionRadius, Vector3.down,
                               out lowerhit, towerData.DetectionRange, towerData.EnemyLayer))
        {
            enemyDetected = true;
        }
        else
        {
            enemyDetected = false;
        }

        if (enemyDetected)
        {
            animator.SetTrigger(actionAnim);

            attackTimer = 0;

            float angleStep = (towerData.NumberOfShots > 1) ? towerData.ShotSpread / (towerData.NumberOfShots - 1) : 0f;

            float startAngle = (-towerData.ShotSpread / 2f) + 90f;

            for (int i = 0; i < towerData.NumberOfShots; i++)
            {
                float currentAngle = startAngle + (angleStep * i);

                Quaternion bulletRotation = transform.rotation * Quaternion.Euler(0, 0, currentAngle);

                CreateAttack(bulletRotation);
            }

            float angleStep2 = (towerData.NumberOfShots > 1) ? towerData.ShotSpread / (towerData.NumberOfShots - 1) : 0f;

            float startAngle2 = (-towerData.ShotSpread / 2f) + -90f;

            for (int i = 0; i < towerData.NumberOfShots; i++)
            {
                float currentAngle2 = startAngle2 + (angleStep2 * i);

                Quaternion bulletRotation2 = transform.rotation * Quaternion.Euler(0, 0, currentAngle2);

                CreateAttack(bulletRotation2);
            }
        }
    }

    private void RadiusDetection()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + towerData.DetectionOriginOffset, towerData.DetectionRange, towerData.EnemyLayer);

        if (cols.Length <= 0)
        {
            enemyDetected = false;
            return;
        }

        HashSet<EnemyController> healths = new HashSet<EnemyController>();

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].TryGetComponent<EnemyController>(out EnemyController health))
            {
                healths.Add(health);
            }
        }

        if (healths.Count <= 0) return;

        enemyDetected = true;

        animator.SetTrigger(actionAnim);

        attackTimer = 0;

        float furthestDistance = -99999999;
        EnemyController furthestEnemy = null;

        foreach (EnemyController health in healths)
        {
            if (health.transform.position.x > furthestDistance)
            {
                furthestDistance = health.transform.position.x;
                furthestEnemy = health;
            }
        }

        if (furthestEnemy == null) return;

        Vector2 targetPos = furthestEnemy.transform.position;
        Vector2 targetVel = Vector2.right * (furthestEnemy.EnemyData.MoveSpeed * (furthestEnemy.TowerDetected ? 0f : 1f));
        Vector2 firePos = transform.position + towerData.DetectionOriginOffset;

        Vector2 predictedPosition = CalculateInterceptPosition(firePos, towerData.AttackPrefab.AttackData.MoveSpeed, targetPos, targetVel);
        Vector2 baseDirection = (predictedPosition - firePos).normalized;

        // Calculate the starting angle shift (half of the total spread)
        float startAngleOffset = -towerData.ShotSpread / 2f;

        // Calculate the step angle between each bullet
        float angleStep = (towerData.NumberOfShots > 1) ? towerData.ShotSpread / (towerData.NumberOfShots - 1) : 0f;

        for (int i = 0; i < towerData.NumberOfShots; i++)
        {
            float currentAngleOffset = startAngleOffset + (angleStep * i);

            // Rotate the base direction vector by the offset angle
            Vector2 bulletDirection = RotateVector(baseDirection, currentAngleOffset);

            CreateAttack(bulletDirection);
        }
    }

    private void ForwardConeDetection()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + towerData.DetectionOriginOffset, towerData.DetectionRange, towerData.EnemyLayer);

        if (cols.Length <= 0)
        {
            enemyDetected = false;
            return;
        }

        HashSet<EnemyController> healths = new HashSet<EnemyController>();

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].TryGetComponent<EnemyController>(out EnemyController health))
            {
                Vector3 dir = health.transform.position - (transform.position + towerData.DetectionOriginOffset);
                float angle = Vector3.Angle(-transform.right, dir);

                if (angle > towerData.DetectionAngle / 2) continue;

                healths.Add(health);
            }
        }

        if (healths.Count <= 0) return;

        enemyDetected = true;

        animator.SetTrigger(actionAnim);

        attackTimer = 0;

        float furthestDistance = -99999999;
        EnemyController furthestEnemy = null;

        foreach (EnemyController health in healths)
        {
            if (health.transform.position.x > furthestDistance)
            {
                furthestDistance = health.transform.position.x;
                furthestEnemy = health;
            }
        }

        if (furthestEnemy == null) return;

        Vector2 targetPos = furthestEnemy.transform.position;
        Vector2 targetVel = Vector2.right * (furthestEnemy.EnemyData.MoveSpeed * (furthestEnemy.TowerDetected ? 0f : 1f));
        Vector2 firePos = transform.position + towerData.DetectionOriginOffset;

        Vector2 predictedPosition = CalculateInterceptPosition(firePos, towerData.AttackPrefab.AttackData.MoveSpeed, targetPos, targetVel);
        Vector2 baseDirection = (predictedPosition - firePos).normalized;

        // Calculate the starting angle shift (half of the total spread)
        float startAngleOffset = -towerData.ShotSpread / 2f;

        // Calculate the step angle between each bullet
        float angleStep = (towerData.NumberOfShots > 1) ? towerData.ShotSpread / (towerData.NumberOfShots - 1) : 0f;

        for (int i = 0; i < towerData.NumberOfShots; i++)
        {
            float currentAngleOffset = startAngleOffset + (angleStep * i);

            // Rotate the base direction vector by the offset angle
            Vector2 bulletDirection = RotateVector(baseDirection, currentAngleOffset);

            CreateAttack(bulletDirection);
        }
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
        if (enemyDetected)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawWireSphere(transform.position + towerData.DetectionOriginOffset, towerData.DetectionRange);
    }

    private void GizmosForwardCone()
    {
        if (enemyDetected)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawWireSphere(transform.position + towerData.DetectionOriginOffset, towerData.DetectionRange);

        Gizmos.DrawRay(transform.position + towerData.DetectionOriginOffset, Quaternion.Euler(0, 0, towerData.DetectionAngle / 2) * -transform.right * towerData.DetectionRange);
        Gizmos.DrawRay(transform.position + towerData.DetectionOriginOffset, Quaternion.Euler(0, 0, -towerData.DetectionAngle / 2) * -transform.right * towerData.DetectionRange);
    }

    #endregion

    private void CreateAttack(Vector3 direction)
    {
        AttackObject instance = null;
        if (AttackPool.Instance != null)
        {
            instance = AttackPool.Instance.GetAttack(towerData.AttackPrefab, transform.position,
                                                   Quaternion.identity);
        }
        else instance = Instantiate(towerData.AttackPrefab, transform.position,
                                                   Quaternion.identity);

        instance.gameObject.SetActive(true);
        instance.InitializeProjectile(direction, towerData.DetectionRange + towerData.DetectionRadius);
    }

    private void CreateAttack(Quaternion direction)
    {
        AttackObject instance = null;
        if (AttackPool.Instance != null)
        {
            instance = AttackPool.Instance.GetAttack(towerData.AttackPrefab, transform.position,
                                                   direction);
        }
        else instance = Instantiate(towerData.AttackPrefab, transform.position,
                                                   direction);

        instance.gameObject.SetActive(true);
        instance.InitializeProjectile(instance.transform.right, towerData.DetectionRange + towerData.DetectionRadius);
    }

    private Vector2 CalculateInterceptPosition(Vector2 shooterPos, float bSpeed, Vector2 tPos, Vector2 tVel)
    {
        Vector2 targetToShooter = tPos - shooterPos;

        // Quadratic equation coefficients: a*t^2 + b*t + c = 0
        float a = Vector2.Dot(tVel, tVel) - (bSpeed * bSpeed);
        float b = 2f * Vector2.Dot(tVel, targetToShooter);
        float c = Vector2.Dot(targetToShooter, targetToShooter);

        float discriminant = (b * b) - (4f * a * c);

        if (discriminant < 0)
        {
            // No algebraic solution possible (target is moving too fast / away). Fallback to current position.
            return tPos;
        }

        // Use the quadratic formula to find time (t)
        float t1 = (-b + Mathf.Sqrt(discriminant)) / (2f * a);
        float t2 = (-b - Mathf.Sqrt(discriminant)) / (2f * a);

        // We want the smallest positive time
        float t = Mathf.Min(t1, t2);
        if (t < 0) t = Mathf.Max(t1, t2);
        if (t < 0) return tPos; // Interception happens in the past; fallback

        // Future location = Current Position + (Velocity * Time)
        return tPos + (tVel * t);
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;

        return new Vector2((cos * tx) - (sin * ty), (sin * tx) + (cos * ty));
    }
}
