using Alchemy.Inspector;
using UnityEngine;

public class AO_HitScan : AttackObject
{
    [field: BoxGroup("Components"), SerializeField] public LineRenderer lineRenderer { get; private set; }

    private RaycastHit[] rayHits;
    private RaycastHit singleHit;

    public override void InitializeProjectile(Vector3 direction, float range)
    {
        lineRenderer.positionCount = 0;

        this.range = range;

        this.direction = direction;
        returned = false;
        entitiesHit.Clear();
        amountOfHits = 0;
        active = true;
        lifeTimer = 0;

        ExecuteAttack();
    }

    public override void StopAttack()
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

    public virtual void ExecuteAttack()
    {
        lineRenderer.positionCount = 2;

        if (AttackData.Pierce > 0)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + (direction * range));

            rayHits = Physics.SphereCastAll(transform.position, AttackData.Radius, direction, range, AttackData.TargetMask);

            foreach (var hit in rayHits)
            {
                if (amountOfHits > AttackData.Pierce) return;

                if (hit.collider.TryGetComponent<HealthController>(out HealthController health))
                {
                    health.TakeDamage(new AttackInfo(AttackData.Damage));

                    PoolableEffect instance = null;
                    if (ParticlesPool.Instance != null)
                    {
                        instance = ParticlesPool.Instance.GetEffect(AttackData.HitParticle, hit.point,
                                                               Quaternion.identity);
                    }
                    else instance = Instantiate(AttackData.HitParticle, hit.point,
                                                               Quaternion.identity);

                    instance.PlayEffect();

                    amountOfHits++;
                }
            }
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);

            if (Physics.SphereCast(transform.position, AttackData.Radius, direction, out singleHit, range, AttackData.TargetMask))
            {
                lineRenderer.SetPosition(1, singleHit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, transform.position + (direction * range));
            }

            if (singleHit.collider.TryGetComponent<HealthController>(out HealthController health))
            {
                health.TakeDamage(new AttackInfo(AttackData.Damage));

                PoolableEffect instance = null;
                if (ParticlesPool.Instance != null)
                {
                    instance = ParticlesPool.Instance.GetEffect(AttackData.HitParticle, singleHit.point,
                                                           Quaternion.identity);
                }
                else instance = Instantiate(AttackData.HitParticle, singleHit.point,
                                                           Quaternion.identity);

                instance.PlayEffect();
            }
        }
    }

    protected override void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGamePaused) return;

        if (!active) return;

        lifeTimer += Time.deltaTime * GameManager.Instance.GameSpeed;

        if (lifeTimer > AttackData.LifeTime) StopAttack();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        
    }
}
