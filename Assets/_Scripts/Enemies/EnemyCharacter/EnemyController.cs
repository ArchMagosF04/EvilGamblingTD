using Alchemy.Inspector;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthController))]
public class EnemyController : MonoBehaviour
{
    [field: BoxGroup("Components"), SerializeField] public SO_EnemyData EnemyData { get; private set; }
    [BoxGroup("Components"), SerializeField] private HealthController healthController;
    [BoxGroup("Components"), SerializeField] private SpriteRenderOrder spriteRenderOrder;
    [BoxGroup("Components"), SerializeField] private Animator animator;

    [BoxGroup("Debug"), SerializeField] private bool DebugGizmos;

    public bool TowerDetected { get; private set; }
    private float towerDetectionTimer;
    private float baseDetectionTimer;

    private float attackTimer;

    public Action OnRemoveEnemyFromWave;
    private bool returned;
    private bool dead;

    private Vector3 detectionCubeCenter = Vector3.zero;
    private Vector3 detectionCubeSize = Vector3.zero;

    public static readonly int moveAnim = Animator.StringToHash("Moving");
    public static readonly int attackAnim = Animator.StringToHash("Attack");
    public static readonly int deathAnim = Animator.StringToHash("Dead");

    private void Awake()
    {
        if (!healthController) healthController = GetComponent<HealthController>();
        if (!spriteRenderOrder) spriteRenderOrder = GetComponentInChildren<SpriteRenderOrder>();
        if (!animator) animator = GetComponentInChildren<Animator>();

        healthController.OnHealthDepleted += ()=> DestroyEnemy(true);

        if (AttackPool.Instance != null && EnemyData.AttackPrefab != null) AttackPool.Instance.PreWarmPool(EnemyData.AttackPrefab, 20);
    }

    private void OnEnable()
    {
        dead = false;
        animator.SetBool(deathAnim, false);
        animator.SetBool(moveAnim, true);

        returned = false;
        TowerDetected = false;
        spriteRenderOrder.UpdateOrderOfLayers();
        healthController.InitializeHealth(EnemyData.MaxHealth);
        towerDetectionTimer = 0;
        attackTimer = 0;
        baseDetectionTimer = 0;
    }

    private void Update()
    {
        if((GameManager.Instance != null && GameManager.Instance.IsGamePaused) || dead) return;

        attackTimer += Time.deltaTime * GameManager.Instance.GameSpeed;
        towerDetectionTimer += Time.deltaTime * GameManager.Instance.GameSpeed;
        baseDetectionTimer += Time.deltaTime * GameManager.Instance.GameSpeed;

        if (!TowerDetected)
        {
            transform.Translate(Vector3.right * EnemyData.MoveSpeed * Time.deltaTime * GameManager.Instance.GameSpeed);
        }
        else
        {
            EnemyAttack();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGamePaused) return;

        TowerDetection();
        BaseDetection();
    }

    #region Detection

    private void TowerDetection()
    {
        if (towerDetectionTimer > EnemyData.DetectionTickTime)
        {
            towerDetectionTimer = 0;

            detectionCubeCenter = new Vector3(transform.position.x + (EnemyData.DetectionRange / 2),
            transform.position.y, transform.position.z);

            detectionCubeSize = new Vector3(EnemyData.DetectionRange, EnemyData.DetectionRadius, 0.2f);

            if (Physics.SphereCast(transform.position, EnemyData.DetectionRadius, Vector3.right, out RaycastHit hitInfo, EnemyData.DetectionRange, EnemyData.DetectionMask))
            //if (Physics.BoxCast(detectionCubeCenter, detectionCubeSize / 2, Vector3.right, Quaternion.identity, EnemyData.DetectionRadius, EnemyData.DetectionMask))
            {
                if (!TowerDetected)
                {
                    //Debug.Log("Tower detected");

                    TowerDetected = true;

                    animator.SetBool(moveAnim, false);
                }
            }
            else
            {
                if (TowerDetected)
                {
                    //Debug.Log("No towers");

                    TowerDetected = false;

                    animator.SetBool(moveAnim, true);
                }
            }
        }
    }

    private void BaseDetection()
    {
        if (baseDetectionTimer > EnemyData.BaseDetectionTickTime)
        {
            baseDetectionTimer = 0;

            if (Physics.Raycast(transform.position + EnemyData.BaseDetectionOffset, Vector3.right, EnemyData.BaseDetectionRange, EnemyData.BaseDetectionMask))
            {
                PlayerManager.Instance.DamageBase(EnemyData.BaseDamage);

                DestroyEnemy(false);
            }
        }
    }

    private void EnemyAttack()
    {
        if (!TowerDetected && EnemyData.OnlyAttackOnTowerDetected) return;

        if (attackTimer > EnemyData.AttackSpeed)
        {
            animator.SetTrigger(attackAnim);

            attackTimer = 0;

            AttackObject instance = null;
            if (AttackPool.Instance != null)
            {
                instance = AttackPool.Instance.GetAttack(EnemyData.AttackPrefab, transform.position,
                                                       Quaternion.identity);
            }
            else instance = Instantiate(EnemyData.AttackPrefab, transform.position,
                                                       Quaternion.identity);

            instance.gameObject.SetActive(true);
            instance.InitializeProjectile(Vector3.right, EnemyData.DetectionRange);
        }
    }

    #endregion

    public void ReceiveEnemyData(SO_EnemyData data) => EnemyData = data;

    public void DestroyEnemy(bool gainMoneyForKill = true)
    {
        dead = true;

        OnRemoveEnemyFromWave?.Invoke();
        OnRemoveEnemyFromWave = null;

        if (gainMoneyForKill)
        {
            PlayerManager.Instance.GainMoney(EnemyData.MoneyReward);

            animator.SetBool(deathAnim, true);

            StartCoroutine(DelayDestruction());

            return;
        }

        if (EnemyPool.Instance != null)
        {
            if (!returned)
            {
                EnemyPool.Instance.ReturnToPool(EnemyData.ID, this);
                returned = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DelayDestruction()
    {
        yield return new WaitForSeconds(0.55f);

        if (EnemyPool.Instance != null)
        {
            if (!returned)
            {
                EnemyPool.Instance.ReturnToPool(EnemyData.ID, this);
                returned = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (!DebugGizmos) return;

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, EnemyData.DetectionRadius);

        Vector3 endPosition = transform.position + (transform.right * EnemyData.DetectionRange);
        Gizmos.DrawWireSphere(endPosition, EnemyData.DetectionRadius);

        Gizmos.DrawLine(transform.position + transform.forward * EnemyData.DetectionRadius, endPosition + transform.forward * EnemyData.DetectionRadius);
        Gizmos.DrawLine(transform.position - transform.forward * EnemyData.DetectionRadius, endPosition - transform.forward * EnemyData.DetectionRadius);
        Gizmos.DrawLine(transform.position + transform.up * EnemyData.DetectionRadius, endPosition + transform.up * EnemyData.DetectionRadius);
        Gizmos.DrawLine(transform.position - transform.up * EnemyData.DetectionRadius, endPosition - transform.up * EnemyData.DetectionRadius);

        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position + EnemyData.BaseDetectionOffset, 
                       (transform.position + EnemyData.BaseDetectionOffset) + (Vector3.right * EnemyData.BaseDetectionRange));
    }
}
