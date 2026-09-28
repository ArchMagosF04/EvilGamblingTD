using UnityEngine;

public class GameWaveSpeedButton : MonoBehaviour
{
    public void GameSpeedPress()
    {
        if (!EnemyWavesManager.Instance.WaveInProgress)
        {
            GameManager.Instance.SetGameToNormalSpeed();
            EnemyWavesManager.Instance.StartNextWave();
        }
        else
        {
            if (GameManager.Instance.GameSpeed == 1) GameManager.Instance.SetGameToDoubleSpeed();
            else if (GameManager.Instance.GameSpeed == 2) GameManager.Instance.SetGameToNormalSpeed();
        }
    }
}
