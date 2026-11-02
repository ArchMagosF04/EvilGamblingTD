using UnityEngine;

public class GameWaveSpeedButton : MonoBehaviour
{
    [SerializeField] GameObject SpeedUpImg;

    private void Awake()
    {
       SpeedUpImg.SetActive(false);  
    }

    public void GameSpeedPress()
    {
        if (!EnemyWavesManager.Instance.WaveInProgress)
        {
            GameManager.Instance.SetGameToNormalSpeed();
            EnemyWavesManager.Instance.StartNextWave();
        }
        else
        {
            if (GameManager.Instance.GameSpeed == 1)
            {
                GameManager.Instance.SetGameToDoubleSpeed();
                SpeedUpImg?.SetActive(true);
            }
            else if (GameManager.Instance.GameSpeed == 2) 
            {
                GameManager.Instance.SetGameToNormalSpeed();
                SpeedUpImg?.SetActive(false);
            } 
        }
    }
}
