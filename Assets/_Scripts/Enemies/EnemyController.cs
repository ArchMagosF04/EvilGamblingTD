using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [field: SerializeField] public int ID { get; private set; }
    [Space(10)]

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float health = 5f;
    [SerializeField] private float damage = 1f;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.linearVelocityX = -speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name + "A");

        if (collision.gameObject.CompareTag("PlayerBase"))
        {
            PlayerManager.Instance.DamageBase(damage);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name + "B");

        if (collision.gameObject.CompareTag("PlayerBase"))
        {
            PlayerManager.Instance.DamageBase(damage);

            Destroy(gameObject);
        }
    }
}
