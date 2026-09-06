using Alchemy.Inspector;
using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public class EnemyController : MonoBehaviour
{
    [field: BoxGroup("Components"), SerializeField] public SO_EnemyData EnemyData { get; private set; }
    [BoxGroup("Components"), SerializeField] private Rigidbody rb;

    public Action OnRemoveEnemyFromWave;
    private bool returned;

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        returned = false;
        rb.linearVelocity = Vector3.right * EnemyData.MoveSpeed;
    }

    public void ReceiveEnemyData(SO_EnemyData data) => EnemyData = data;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name + "A");

        if (collision.gameObject.CompareTag("PlayerBase"))
        {
            PlayerManager.Instance.DamageBase(EnemyData.Damage);

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
}
