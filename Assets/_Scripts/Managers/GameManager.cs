using Alchemy.Inspector;
using Ami.BroAudio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field:SerializeField, ReadOnly] public bool IsGamePaused {  get; private set; }

    [field: SerializeField, ReadOnly] public float GameSpeed { get; private set; } = 1f;

    [SerializeField] private SoundID levelMusic = default;

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

        SetGameToNormalSpeed();

        Time.timeScale = 1f;

        BroAudio.Play(levelMusic);
    }

    public void ToggleGamePause(bool value)
    {
        IsGamePaused = value;
    }

    public void SetGameToDoubleSpeed()
    {
        GameSpeed = 2f;
    }

    public void SetGameToNormalSpeed()
    {
        GameSpeed = 1f;
    }

    public void SetGameToHalfSpeed()
    {
        GameSpeed = 0.5f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
}
