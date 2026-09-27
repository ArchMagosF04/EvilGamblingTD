using Alchemy.Inspector;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PoolableEffect : MonoBehaviour
{
    [BoxGroup("Main Effect"), SerializeField] private ParticleSystem particle;
    [BoxGroup("Main Effect"), SerializeField] private VisualEffect vfx;

    [field: SerializeField] public string ID { get; private set; }

    private bool returned;

    private WaitForSeconds delay = new(0.1f);

    private void Awake()
    {
        if (particle == null) particle = GetComponent<ParticleSystem>();
        if (vfx == null) vfx = GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        returned = false;
    }

    public void PlayEffect()
    {
        if (particle != null)
        {
            particle.Play();
        }
        if (vfx != null)
        {
            vfx.Play();
            StartCoroutine(WaitForVFXEnd());
        }
    }

    private void OnParticleSystemStopped()
    {
        EndEffect();
    }

    private IEnumerator WaitForVFXEnd()
    {
        yield return delay;

        // Check if any sub-systems are active or have alive particles
        while (vfx.HasAnySystemAwake())
        {
            yield return delay;
        }

        EndEffect();
    }

    private void EndEffect()
    {
        if (ParticlesPool.Instance != null)
        {
            if (!returned)
            {
                ParticlesPool.Instance.ReturnToPool(ID, this);
                returned = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
