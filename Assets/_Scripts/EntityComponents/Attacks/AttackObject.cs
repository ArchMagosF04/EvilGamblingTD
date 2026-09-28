using Alchemy.Inspector;
using System.Collections.Generic;
using UnityEngine;

public class AttackObject : MonoBehaviour
{
    [field: BoxGroup("Components"), SerializeField] public SO_Attack AttackData { get; private set; }

    protected HashSet<HealthController> entitiesHit = new HashSet<HealthController>();

    protected int amountOfHits;
    protected bool returned;
    protected Vector3 direction;
    protected bool active;
    protected float lifeTimer;
    protected float range;
    protected Vector3 startingPoint;

    public virtual void InitializeProjectile(Vector3 direction, float range)
    {
        this.range = range;
        startingPoint = transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        this.direction = direction;
        returned = false;
        entitiesHit.Clear();
        amountOfHits = 0;
        active = true;
        lifeTimer = 0;

    }

    public virtual void StopAttack()
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

    protected virtual void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGamePaused) return;

        if (!active) return;

        transform.position += direction * AttackData.MoveSpeed * Time.deltaTime * GameManager.Instance.GameSpeed;

        lifeTimer += Time.deltaTime * GameManager.Instance.GameSpeed;

        if (lifeTimer > AttackData.LifeTime ||
            (AttackData.DestroyBeyondRange && Vector3.Distance(transform.position, startingPoint) > range))
        {
            if (AttackData.DoEffectOnDeath)
            {
                PoolableEffect instance = null;
                if (ParticlesPool.Instance != null)
                {
                    instance = ParticlesPool.Instance.GetEffect(AttackData.HitParticle, transform.position,
                                                           Quaternion.identity);
                }
                else instance = Instantiate(AttackData.HitParticle, transform.position,
                                                           Quaternion.identity);

                instance.PlayEffect();
            }

            StopAttack();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlockAttacks"))
        {
            StopAttack();
        }

        if (amountOfHits > AttackData.Pierce) return;

        if (other.TryGetComponent<HealthController>(out HealthController health))
        {
            if (!AttackData.CanHitSameEnemyMultipleTimes)
            {
                if (entitiesHit.Contains(health)) return;
            }

            entitiesHit.Add(health);

            health.TakeDamage(new AttackInfo(AttackData.Damage));

            PoolableEffect instance = null;
            if (ParticlesPool.Instance != null)
            {
                instance = ParticlesPool.Instance.GetEffect(AttackData.HitParticle, other.transform.position,
                                                       Quaternion.identity);
            }
            else instance = Instantiate(AttackData.HitParticle, other.transform.position,
                                                       Quaternion.identity);

            instance.PlayEffect();

            amountOfHits++;

            if (amountOfHits > AttackData.Pierce)
            {
                if (AttackData.DestroyOnLastHit) StopAttack();
            }
        }
    }
}
