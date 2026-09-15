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
    private float startTime;
    private bool hasAttacked;

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
        startTime = Time.time;
        hasAttacked = false;
    }

    private void Update()
    {
        if (!active) return;

        transform.position += direction * AttackData.MoveSpeed * Time.deltaTime;

        if (Time.time > startTime + AttackData.LifeTime) StopAttack();
    }

    private void FixedUpdate()
    {
        if (!active) return;

        if (!AttackData.HitsMultipleEnemies)
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
        if (AttackData.AttacksOnlyOnce && hasAttacked) return;

        hasAttacked = true;

        RaycastHit hit = projectileCollider.GetSingleCollision();

        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<HealthController>(out HealthController health))
        {
            if (!AttackData.CanHitMultipleTimes)
            {
                if (entitiesHit.Contains(health)) return;
            }

            entitiesHit.Add(health);

            health.TakeDamage(new AttackInfo(AttackData.Damage));

            amountOfHits++;
        }

        if (AttackData.Pierce > 0 && amountOfHits >= AttackData.Pierce)
        {
            if (AttackData.DestroyOnLastHit) StopAttack();
        }
    }

    private void MultiCollisionAttack()
    {
        if (AttackData.AttacksOnlyOnce && hasAttacked) return;

        hasAttacked = true;

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
                    if (AttackData.DestroyOnLastHit)
                    {
                        StopAttack();
                        return;
                    }
                }
            }
        }
    }

    public void StopAttack()
    {
        active = false;

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
