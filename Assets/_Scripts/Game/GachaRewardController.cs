using Alchemy.Inspector;
using UnityEngine;
using UnityEngine.Events;

public class GachaRewardController : MonoBehaviour
{
    [SerializeField] private SO_PlayerSavedData savedData;

    [BoxGroup("Components"), SerializeField] private Canvas milestoneCanvas;

    [BoxGroup("Gacha Coin Reward"), SerializeField] private int flatRewardPerWave = 5;
    [BoxGroup("Gacha Coin Reward"), SerializeField] private float baseMilestoneMult = 1.1f;
    [BoxGroup("Gacha Coin Reward"), SerializeField] private float milestoneMultScaling = 1.1f;

    [BoxGroup("Milestone"), SerializeField] private int wavesPerMilestone = 10;

    [BoxGroup("Event Channels"), SerializeField] private StringEvent defeatRewardText;

    private int currentGachaCoinsCollected;
    private int currentWaveNumber;

    private int waveStreak;
    private int milestoneStreak;

    private void Start()
    {
        currentGachaCoinsCollected = 0;
    }

    public void OnWaveEnd(int wave)
    {
        currentGachaCoinsCollected += flatRewardPerWave;
        currentWaveNumber = wave;
    }

    public void PlayerDefeated()
    {
        defeatRewardText.InvokeEvent(currentGachaCoinsCollected.ToString());
    }

    public void RedeemDefeatedReward()
    {
        savedData.ReceiveGachaCoins(currentGachaCoinsCollected);
    }

    public void RedeemMilestoneReward()
    {

    }
}
