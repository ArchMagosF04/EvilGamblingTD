using Alchemy.Inspector;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [field: BoxGroup("Components"), SerializeField] public SO_Attack AttackData { get; private set; }
    [field: BoxGroup("Components"), SerializeField] public IProjectileCollider projectileCollider { get; private set; }

    private HashSet<HealthController> entitiesHit = new HashSet<HealthController>();
    private int amountOfHits;
    private bool returned;
    private Vector3 direction;
    private bool active;

    private void Awake()
    {
        if (projectileCollider == null) projectileCollider = GetComponent<IProjectileCollider>();

        projectileCollider.GiveAttackData(AttackData);
    }

    public void InitializeProjectile(Vector3 direction)
    {
        transform.right = direction;
        this.direction = direction;
        returned = false;
        entitiesHit.Clear();
        amountOfHits = 0;
        active = true;
    }

    private void Update()
    {
        if (!active) return;

        transform.position += direction * AttackData.MoveSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!active) return;

        if (AttackData.Pierce <= 0)
        {
            SingleCollisionAttack();
        }
        else
        {
            MultiCollisionAttack();
        }
    }

    private void SingleCollisionAttack()
    {
        RaycastHit hit = projectileCollider.GetSingleCollision();

        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<HealthController>(out HealthController health))
        {
            health.TakeDamage(new AttackInfo(AttackData.Damage));

            StopAttack();
        }
    }

    private void MultiCollisionAttack()
    {
        RaycastHit[] hits = projectileCollider.GetMultipleCollisions();

        if (hits.Length <= 0) return;

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null) continue;

            if (hit.collider.TryGetComponent<HealthController>(out HealthController health))
            {
                if (!AttackData.CanHitMultipleTimes)
                {
                    if (entitiesHit.Contains(health)) continue;
                }

                entitiesHit.Add(health);

                health.TakeDamage(new AttackInfo(AttackData.Damage));

                amountOfHits++;

                if (AttackData.Pierce > 0 && amountOfHits >= AttackData.Pierce)
                {
                    StopAttack();
                }
            }
        }
    }

    public void StopAttack()
    {
        if (AttackPool.Instance != null)
        {
            if (!returned)
            {
                AttackPool.Instance.ReturnToPool(AttackData.ID, this);
                returned = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
