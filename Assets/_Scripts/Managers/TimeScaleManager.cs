using System;
using System.Collections;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public static TimeScaleManager Instance;

    public bool IsGamePaused { get; private set; }

    private Coroutine hitStopCoroutine;
    private Coroutine slowDownCoroutine;

    private bool isHitStopActive;
    private bool isTimeSlowed;

    public Action OnTimeSlowEnd;
    public Action OnHitStopEnd;

    private bool useHitStopEndAction;
    private bool useTimeSlowEndAction;

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

    public void PauseGameTime()
    {
        IsGamePaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGameTime()
    {
        IsGamePaused = false;
        if (!isHitStopActive && !isTimeSlowed) Time.timeScale = 1f;
    }

    public void ForceResumeGameTime()
    {
        IsGamePaused = false;
        Time.timeScale = 1f;
        ForceEndSlowDown();
        ForceEndHitStop();
    }

    public void DoHitStop(float duration, Action onEndAction = null)
    {
        if (hitStopCoroutine != null)
        {
            if (isHitStopActive) OnHitStopInterrupt();

            StopCoroutine(hitStopCoroutine);
        }

        if (onEndAction != null)
        {
            useHitStopEndAction = true;
            OnHitStopEnd = onEndAction;
        }

        hitStopCoroutine = StartCoroutine(HitStopRoutine(duration));
    }

    private IEnumerator HitStopRoutine(float duration)
    {
        isHitStopActive = true;
        Time.timeScale = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (!IsGamePaused) elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = 1f;
        isHitStopActive = false;

        OnHitStopInterrupt();
    }

    public void ForceEndHitStop()
    {
        useHitStopEndAction = false;
        OnHitStopEnd = null;

        if (hitStopCoroutine != null) StopCoroutine(hitStopCoroutine);
        Time.timeScale = 1f;
        isHitStopActive = false;
    }

    public void SlowDownTime(float slowAmount, float duration, Action onEndAction = null)
    {
        if (slowDownCoroutine != null)
        {
            if (isTimeSlowed) OnTimeSlowInterrupt();

            StopCoroutine(slowDownCoroutine);
        }

        if (onEndAction != null)
        {
            useTimeSlowEndAction = true;
            OnTimeSlowEnd = onEndAction;
        }

        slowDownCoroutine = StartCoroutine(SlowDownRoutine(slowAmount, duration));
    }

    private IEnumerator SlowDownRoutine(float slowAmount, float duration)
    {
        isTimeSlowed = true;
        Time.timeScale = slowAmount;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (!IsGamePaused) elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = 1f;
        isTimeSlowed = false;

        OnTimeSlowInterrupt();
    }

    public void ForceEndSlowDown()
    {
        useTimeSlowEndAction = false;
        OnTimeSlowEnd = null;
        if (slowDownCoroutine != null) StopCoroutine(slowDownCoroutine);
        Time.timeScale = 1f;
        isTimeSlowed = false;
    }

    private void OnHitStopInterrupt()
    {
        if (useHitStopEndAction)
        {
            useHitStopEndAction = false;
            OnHitStopEnd?.Invoke();
            OnHitStopEnd = null;
        }
    }

    private void OnTimeSlowInterrupt()
    {
        if (useTimeSlowEndAction)
        {
            useTimeSlowEndAction = false;
            OnTimeSlowEnd?.Invoke();
            OnTimeSlowEnd = null;
        }
    }
}
