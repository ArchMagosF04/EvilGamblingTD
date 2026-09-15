using Alchemy.Inspector;
using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(HealthController))]
public class EnemyController : MonoBehaviour
{
    [field: BoxGroup("Components"), SerializeField] public SO_EnemyData EnemyData { get; private set; }
    [BoxGroup("Components"), SerializeField] private HealthController healthController;
    [BoxGroup("Components"), SerializeField] private SpriteRenderOrder spriteRenderOrder;

    [BoxGroup("Debug"), SerializeField] private bool DebugGizmos;

    private bool towerDetected;
    private float towerDetectionLastTick;
    private float baseDetectionLastTick;

    private float lastAttackTime;

    public Action OnRemoveEnemyFromWave;
    private bool returned;

    private Vector3 detectionCubeCenter = Vector3.zero;
    private Vector3 detectionCubeSize = Vector3.zero;

    private void Awake()
    {
        if (!healthController) healthController = GetComponent<HealthController>();
        if (!spriteRenderOrder) spriteRenderOrder = GetComponentInChildren<SpriteRenderOrder>();

        healthController.OnHealthDepleted += ()=> DestroyEnemy(true);

        if (AttackPool.Instance != null) AttackPool.Instance.PreWarmPool(EnemyData.AttackPrefab, 20);
    }

    private void OnEnable()
    {
        returned = false;
        towerDetected = false;
        spriteRenderOrder.UpdateOrderOfLayers();
        healthController.InitializeHealth(EnemyData.MaxHealth);
    }

    private void Update()
    {
        if (!towerDetected)
        {
            transform.Translate(Vector3.right * EnemyData.MoveSpeed * Time.deltaTime);
        }
        else
        {
            EnemyAttack();
        }
    }

    private void FixedUpdate()
    {
        TowerDetection();
        BaseDetection();
    }

    #region Detection

    private void TowerDetection()
    {
        if (Time.time > towerDetectionLastTick + EnemyData.DetectionTickTime)
        {
            towerDetectionLastTick = Time.time;

            detectionCubeCenter = new Vector3(transform.position.x + (EnemyData.DetectionRange / 2),
            transform.position.y, transform.position.z);

            detectionCubeSize = new Vector3(EnemyData.DetectionRange, EnemyData.DetectionRadius, 0.2f);

            //if (Physics.SphereCast(transform.position, EnemyData.DetectionRadius, Vector3.right, out RaycastHit hitInfo, EnemyData.DetectionRange, EnemyData.DetectionMask))
            if (Physics.BoxCast(detectionCubeCenter, detectionCubeSize / 2, Vector3.right, Quaternion.identity, EnemyData.DetectionRadius, EnemyData.DetectionMask))
            {
                if (!towerDetected)
                {
                    //Debug.Log("Tower detected");

                    towerDetected = true;
                }
            }
            else
            {
                if (towerDetected)
                {
                    //Debug.Log("No towers");

                    towerDetected = false;
                }
            }
        }
    }

    private void BaseDetection()
    {
        if (Time.time > baseDetectionLastTick + EnemyData.BaseDetectionTickTime)
        {
            baseDetectionLastTick = Time.time;

            if (Physics.Raycast(transform.position + EnemyData.BaseDetectionOffset, Vector3.right, EnemyData.BaseDetectionRange, EnemyData.BaseDetectionMask))
            {
                PlayerManager.Instance.DamageBase(EnemyData.BaseDamage);

                DestroyEnemy(false);
            }
        }
    }

    private void EnemyAttack()
    {
        if (!towerDetected && EnemyData.OnlyAttackOnTowerDetected) return;

        if (Time.time > lastAttackTime + EnemyData.AttackSpeed)
        {
            lastAttackTime = Time.time;

            Projectile instance = null;
            if (AttackPool.Instance != null)
            {
                instance = AttackPool.Instance.GetAttack(EnemyData.AttackPrefab, transform.position,
                                                       Quaternion.identity);
            }
            else instance = Instantiate(EnemyData.AttackPrefab, transform.position,
                                                       Quaternion.identity);

            instance.gameObject.SetActive(true);
            instance.InitializeProjectile(Vector3.right);
        }
    }

    #endregion

    public void ReceiveEnemyData(SO_EnemyData data) => EnemyData = data;

    public void DestroyEnemy(bool gainMoneyForKill = true)
    {
        OnRemoveEnemyFromWave?.Invoke();
        OnRemoveEnemyFromWave = null;

        if (gainMoneyForKill)
        {
            PlayerManager.Instance.GainMoney(EnemyData.MoneyReward);
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

    private void OnDrawGizmos()
    {
        if (!DebugGizmos) return;

        Gizmos.color = Color.cyan;

        detectionCubeCenter = new Vector3(transform.position.x + (EnemyData.DetectionRange / 2),
            transform.position.y, transform.position.z);

        detectionCubeSize = new Vector3(EnemyData.DetectionRange, EnemyData.DetectionRadius, 0.2f);

        DebugBoxCast.SimpleDrawBoxCast(transform.position, detectionCubeSize / 2, Quaternion.identity, transform.right, EnemyData.DetectionRange, Color.cyan);

        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position + EnemyData.BaseDetectionOffset, 
                       (transform.position + EnemyData.BaseDetectionOffset) + (Vector3.right * EnemyData.BaseDetectionRange));
    }
}
