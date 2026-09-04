using Alchemy.Inspector;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsShape;

public class EnemyController : MonoBehaviour
{
    [field: BoxGroup("Components"), SerializeField] public SO_EnemyData EnemyData { get; private set; }
    [BoxGroup("Components"), SerializeField] private Rigidbody rb;

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector3.right * EnemyData.MoveSpeed;
    }

    public void ReceiveEnemyData(SO_EnemyData data) => EnemyData = data;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + "A");

        if (collision.gameObject.CompareTag("PlayerBase"))
        {
            PlayerManager.Instance.DamageBase(EnemyData.Damage);

            Destroy(gameObject);
        }
    }

    //private void OnTriggerEnter(Collider collision)
    //{
    //    Debug.Log(collision.gameObject.name + "B");

    //    if (collision.gameObject.CompareTag("PlayerBase"))
    //    {
    //        PlayerManager.Instance.DamageBase(EnemyData.Damage);

    //        Destroy(gameObject);
    //    }
    //}
}
