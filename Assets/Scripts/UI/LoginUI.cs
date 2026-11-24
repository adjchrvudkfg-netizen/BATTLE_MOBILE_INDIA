using UnityEngine;
using UnityEngine.UI;
using BattleMobileIndia.Core;

namespace BattleMobileIndia.UI
{
    public class LoginUI : MonoBehaviour
    {
        [Header("UI References")]
        public InputField playerNameInput;
        public Text statusText;
        public Button guestLoginButton;
        public Button startButton;

        private void Awake()
        {
            if (guestLoginButton != null)
            {
                guestLoginButton.onClick.AddListener(OnGuestLoginClicked);
            }

            if (startButton != null)
            {
                startButton.onClick.AddListener(OnEnterLobbyClicked);
            }
        }

        private void OnGuestLoginClicked()
        {
            string enteredName = playerNameInput != null ? playerNameInput.text : "BMI_Guest";
            string randomId = $"BMI_{Random.Range(10000, 99999)}";

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetPlayerSession(randomId, enteredName);
                GameManager.Instance.SetGameState(GameState.Lobby);
            }

            if (statusText != null)
            {
                statusText.text = $"Logged in as {enteredName} ({randomId})";
            }

            SceneFlowController.Instance?.LoadLobby();
        }

        private void OnEnterLobbyClicked()
        {
            OnGuestLoginClicked();
        }
    }
}

