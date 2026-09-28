using Alchemy.Inspector;
using Ami.BroAudio;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Payer Stats")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float startingMoney = 500;

    [Header("Event Channels")]
    [SerializeField] private StringEvent moneyValueText;
    [SerializeField] private StringEvent healthValueText;

    [BoxGroup("Sounds"), SerializeField] private SoundID BaseLoseHealthSound;
    [BoxGroup("Sounds"), SerializeField] private SoundID GameOverSound;

    public UnityEvent OnGameOver;

    private float money;
    private float currentHealth;

    private bool playerLost;

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

        playerLost = false;
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

        BroAudio.Play(BaseLoseHealthSound);

        if (CurrentHealth <= 0 && !playerLost)
        {
            Time.timeScale = 0f;

            playerLost = true;

            CurrentHealth = 0;

            BroAudio.Play(GameOverSound);

            //Game over.

            OnGameOver?.Invoke();
        }
    }

    [Button]
    public void HealBase(float amount)
    {
        CurrentHealth += amount;

        healthValueText?.InvokeEvent("Health: " + CurrentHealth.ToString());
    }
}
