using Alchemy.Inspector;
using UnityEngine;

public class PauseTimeButton : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private GameObject pauseIcon;
    [BoxGroup("Components"), SerializeField] private GameObject resumeIcon;

    private void Start()
    {
        pauseIcon.SetActive(true);
        resumeIcon.SetActive(false);
    }

    public void PauseButtonPress()
    {
        if (GameManager.Instance.IsGamePaused)
        {
            pauseIcon.SetActive(true);
            resumeIcon.SetActive(false);

            GameManager.Instance.ToggleGamePause(false);
        }
        else
        {
            pauseIcon.SetActive(false);
            resumeIcon.SetActive(true);

            GameManager.Instance.ToggleGamePause(true);
        }
    }
}
