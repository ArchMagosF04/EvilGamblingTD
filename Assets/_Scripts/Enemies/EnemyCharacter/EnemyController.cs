using Alchemy.Inspector;
using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [field: BoxGroup("Components"), SerializeField] public SO_EnemyData EnemyData { get; private set; }
    [BoxGroup("Components"), SerializeField] private Rigidbody rb;
    [BoxGroup("Components"), SerializeField] private SpriteRenderOrder spriteRenderOrder;

    [BoxGroup("Debug"), SerializeField] private bool DebugGizmos;

    private bool towerDetected;
    private float towerDetectionLastTick;

    public Action OnRemoveEnemyFromWave;
    private bool returned;

    private void Awake()
    {
        if (!spriteRenderOrder) spriteRenderOrder = GetComponentInChildren<SpriteRenderOrder>();
        if (!rb) rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        returned = false;
        towerDetected = false;
        rb.linearVelocity = Vector3.right * EnemyData.MoveSpeed;
        spriteRenderOrder.UpdateOrderOfLayers();
    }

    Vector3 detectionCubeCenter = Vector3.zero;
    Vector3 detectionCubeSize = Vector3.zero;

    private void FixedUpdate()
    {
        MovementTowerDetection();
    }

    private void MovementTowerDetection()
    {
        if (Time.time > towerDetectionLastTick + EnemyData.DetectionTickTime)
        {
            towerDetectionLastTick = Time.time;

            detectionCubeCenter = new Vector3(transform.position.x + (EnemyData.DetectionRange / 2),
            transform.position.y, transform.position.z);

            detectionCubeSize = new Vector3(EnemyData.DetectionRange / 2, EnemyData.DetectionRadius / 2, 1f);

            if (Physics.SphereCast(transform.position, EnemyData.DetectionRadius, Vector3.right, out RaycastHit hitInfo ,EnemyData.DetectionRange ,EnemyData.DetectionMask))
            //if (Physics.BoxCast(detectionCubeCenter, detectionCubeSize, Vector3.right, Quaternion.identity, EnemyData.DetectionRadius, EnemyData.DetectionMask))
            {
                if (!towerDetected)
                {
                    Debug.Log("Tower detected");

                    towerDetected = true;

                    rb.linearVelocity = Vector3.zero;
                }
            }
            else
            {
                if (towerDetected)
                {
                    Debug.Log("No towers");

                    towerDetected = false;

                    rb.linearVelocity = Vector3.right * EnemyData.MoveSpeed;
                }
            }
        }
    }

    private void EnemyAttack()
    {
        if (!towerDetected) return;


    }

    public void ReceiveEnemyData(SO_EnemyData data) => EnemyData = data;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name + "A");

        if (collision.gameObject.CompareTag("PlayerBase"))
        {
            PlayerManager.Instance.DamageBase(EnemyData.BaseDamage);

            DestroyEnemy();
        }
    }

    //private void OnTriggerEnter(Collider collision)
    //{
    //    Debug.Log(collision.gameObject.name + "B");

    //    if (collision.gameObject.CompareTag("PlayerBase"))
    //    {
    //        PlayerManager.Instance.DamageBase(EnemyData.Damage);

    //        DestroyEnemy();
    //    }
    //}

    public void DestroyEnemy()
    {
        OnRemoveEnemyFromWave?.Invoke();
        OnRemoveEnemyFromWave = null;

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

        detectionCubeCenter = new Vector3(transform.position.x + (EnemyData.DetectionRange/2), 
            transform.position.y, transform.position.z);
        detectionCubeSize = new Vector3(EnemyData.DetectionRange, EnemyData.DetectionRadius, 0.01f);

        Gizmos.DrawWireCube(detectionCubeCenter, detectionCubeSize);
    }
}
