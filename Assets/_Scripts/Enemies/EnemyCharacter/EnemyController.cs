using Alchemy.Inspector;
using Ami.BroAudio;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthController))]
public class EnemyController : MonoBehaviour
{
    [field: BoxGroup("Components"), SerializeField] public SO_EnemyData EnemyData { get; private set; } //Contains all data about the enemy.
    [BoxGroup("Components"), SerializeField] private HealthController healthController;
    [BoxGroup("Components"), SerializeField] private SpriteRenderOrder spriteRenderOrder; //Handles sorting order of sprites.
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
        //If components aren't assigned then get them. 
        if (!healthController) healthController = GetComponent<HealthController>();
        if (!spriteRenderOrder) spriteRenderOrder = GetComponentInChildren<SpriteRenderOrder>();
        if (!animator) animator = GetComponentInChildren<Animator>();

        //Subscribe to the death event of the health script to return the enemy to the pool.
        healthController.OnHealthDepleted += ()=> DestroyEnemy(true);

        //Pre warm instances of the enemy's attack to be reused during the game.
        if (AttackPool.Instance != null && EnemyData.AttackPrefab != null) AttackPool.Instance.PreWarmPool(EnemyData.AttackPrefab, 20);
    }

    private void OnEnable()
    {
        dead = false;
        //Reset animator.
        animator.SetBool(deathAnim, false);
        animator.SetBool(moveAnim, true);

        returned = false;
        TowerDetected = false;
        spriteRenderOrder.UpdateOrderOfLayers(); //Apply sorting order of sprites based on Y world position.
        healthController.InitializeHealth(EnemyData.MaxHealth); //Reset the enemies health.
        towerDetectionTimer = 0;
        attackTimer = 0;
        baseDetectionTimer = 0;
    }

    private void Update()
    {
        if((GameManager.Instance != null && GameManager.Instance.IsGamePaused) || dead) return; //Don't update if the game is paused or the enemy is dead.

        //All timers are linked to the speed multipler of the game so they can finish faster when turbo mode is enabled.
        attackTimer += Time.deltaTime * GameManager.Instance.GameSpeed;
        towerDetectionTimer += Time.deltaTime * GameManager.Instance.GameSpeed;
        baseDetectionTimer += Time.deltaTime * GameManager.Instance.GameSpeed;

        TowerDetection();
        BaseDetection();

        if (!TowerDetected) //If there isn't a tower in its attack range then keep moving, else stop to attack towers.
        {
            transform.Translate(Vector3.right * EnemyData.MoveSpeed * Time.deltaTime * GameManager.Instance.GameSpeed);
        }
        else
        {
            EnemyAttack();
        }
    }

    #region Detection

    private void TowerDetection()
    {
        if (towerDetectionTimer > EnemyData.DetectionTickTime) //If the detection tick is up then check detection.
        {
            //Reset timer.
            towerDetectionTimer = 0;

            if (Physics.SphereCast(transform.position, EnemyData.DetectionRadius, Vector3.right, out RaycastHit hitInfo, EnemyData.DetectionRange, EnemyData.DetectionMask))
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
            BroAudio.Play(EnemyData.AttackSound);

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

            BroAudio.Play(EnemyData.DeathSound);

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

    public void PauseOrPlayAnimator(bool toggle)
    {
        if (dead) return;

        if (toggle) animator.speed = 0;
        else animator.speed = 1;
    }
}
