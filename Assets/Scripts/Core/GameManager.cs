using System;
using UnityEngine;

namespace BattleMobileIndia.Core
{
    public enum GameState
    {
        Login,
        Lobby,
        PreMatchCountdown,
        InMatch,
        MatchEnd
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameState CurrentState { get; private set; } = GameState.Login;
        public string ActivePlayerId { get; private set; } = "Guest_001";
        public string ActivePlayerName { get; private set; } = "BMI_Player";

        public event Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetPlayerSession(string playerId, string playerName)
        {
            ActivePlayerId = string.IsNullOrWhiteSpace(playerId) ? "Guest_001" : playerId.Trim();
            ActivePlayerName = string.IsNullOrWhiteSpace(playerName) ? "BMI_Player" : playerName.Trim();
        }

        public void SetGameState(GameState newState)
        {
            if (CurrentState == newState)
            {
                return;
            }

            CurrentState = newState;
            OnGameStateChanged?.Invoke(CurrentState);
        }
    }
}

