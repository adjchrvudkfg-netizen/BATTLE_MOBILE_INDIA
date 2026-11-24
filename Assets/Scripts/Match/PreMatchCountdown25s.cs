using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleMobileIndia.Match
{
    public class PreMatchCountdown25s : MonoBehaviour
    {
        [Header("Countdown Settings")]
        public float startSeconds = 25f;
        public bool autoStartOnEnable = true;

        [Header("Optional UI Text (Legacy Text)")]
        public Text countdownText;

        public event Action OnCountdownFinished;

        public float RemainingSeconds { get; private set; }
        public bool IsRunning { get; private set; }

        private void OnEnable()
        {
            if (autoStartOnEnable)
            {
                StartCountdown(startSeconds);
            }
        }

        public void StartCountdown(float durationSeconds = 25f)
        {
            RemainingSeconds = Mathf.Max(0f, durationSeconds);
            IsRunning = true;
            UpdateUI();
        }

        private void Update()
        {
            if (!IsRunning)
            {
                return;
            }

            RemainingSeconds -= Time.deltaTime;
            if (RemainingSeconds <= 0f)
            {
                RemainingSeconds = 0f;
                IsRunning = false;
                UpdateUI();
                OnCountdownFinished?.Invoke();
                return;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (countdownText != null)
            {
                int secondsCeil = Mathf.CeilToInt(RemainingSeconds);
                countdownText.text = $"Match starts in: {secondsCeil}s";
            }
        }
    }
}

