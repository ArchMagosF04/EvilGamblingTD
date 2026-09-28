using Alchemy.Inspector;
using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    [BoxGroup("Debug"), SerializeField, ReadOnly] private float maxHealth;
    [BoxGroup("Debug"), SerializeField, ReadOnly] private float currentHealth;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead { get; private set; }

    public Action OnHealthDepleted;

    public DamagePopUp damagePopUp;


    public void InitializeHealth(float maxHealth)
    {
        IsDead = false;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    [Button, BoxGroup("Debug")]
    public void TakeDamage(AttackInfo info)
    {
        if (IsDead) return;

        currentHealth -= info.Damage;

        if(damagePopUp != null) 
        {
            DamagePopUp popUp = Instantiate(damagePopUp, transform.position, Quaternion.identity);
            popUp.damage = info.Damage;

        }

        if (currentHealth <= 0)
        {
            IsDead = true;

            currentHealth = 0;

            OnHealthDepleted?.Invoke();
        }
    }
}
