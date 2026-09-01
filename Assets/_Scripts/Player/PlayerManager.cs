using Alchemy.Inspector;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Payer Stats")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float startingMoney = 500;

    [Header("Event Channels")]
    [SerializeField] private StringEvent moneyValueText;
    [SerializeField] private StringEvent healthValueText;

    private float money;
    private float currentHealth;

    public float Money
    {
        get { return money; }
        set { money = Mathf.Clamp(value, 0, 999999999); }
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = Mathf.Clamp(value, 0, maxHealth); }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        CurrentHealth = maxHealth;
        Money = startingMoney;

        moneyValueText?.InvokeEvent("Money: " + Money.ToString());
        healthValueText?.InvokeEvent("Health: " + CurrentHealth.ToString());
    }

    [Button]
    public void GainMoney(float amount)
    {
        Money += amount;

        moneyValueText?.InvokeEvent("Money: " + Money.ToString());
    }

    public void LoseMoney(float amount)
    {
        Money -= amount;

        moneyValueText?.InvokeEvent("Money: " + Money.ToString());
    }

    public void DamageBase(float amount)
    {
        CurrentHealth -= amount;

        healthValueText?.InvokeEvent("Health: " + CurrentHealth.ToString());

        if (CurrentHealth <= 0)
        {
            //Game over.
        }
    }

    [Button]
    public void HealBase(float amount)
    {
        CurrentHealth += amount;

        healthValueText?.InvokeEvent("Health: " + CurrentHealth.ToString());
    }
}
