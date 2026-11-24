using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace BattleMobileIndia.UI
{
    public class FreeFireLoginUI : MonoBehaviour
    {
        [Header("Login UI Elements")]
        public TMP_InputField nicknameInput;
        public Button guestLoginButton;
        public Button googleLoginButton;
        public Button confirmLoginButton;
        public TMP_Text statusText;

        [Header("Scene Flow")]
        public string lobbySceneName = "02_Lobby";

        private const string PlayerNameKey = "BMI_PlayerName";
        private const string LoginMethodKey = "BMI_LoginMethod";

        private void Start()
        {
            if (nicknameInput != null && PlayerPrefs.HasKey(PlayerNameKey))
            {
                nicknameInput.text = PlayerPrefs.GetString(PlayerNameKey);
            }

            if (guestLoginButton != null)
                guestLoginButton.onClick.AddListener(OnGuestLogin);

            if (googleLoginButton != null)
                googleLoginButton.onClick.AddListener(OnGoogleLogin);

            if (confirmLoginButton != null)
                confirmLoginButton.onClick.AddListener(OnConfirmNicknameLogin);

            SetStatus("Tap Guest or Google to enter BATTLE MOBILE INDIA");
        }

        public void OnGuestLogin()
        {
            string randomId = Random.Range(1000, 9999).ToString();
            string guestName = $"BMI_Survivor_{randomId}";
            SaveAndEnterLobby(guestName, "Guest");
        }

        public void OnGoogleLogin()
        {
            string savedOrDefault = (nicknameInput != null && !string.IsNullOrWhiteSpace(nicknameInput.text))
                ? nicknameInput.text.Trim()
                : "BMI_GooglePlayer";

            SaveAndEnterLobby(savedOrDefault, "Google");
        }

        public void OnConfirmNicknameLogin()
        {
            if (nicknameInput == null || string.IsNullOrWhiteSpace(nicknameInput.text))
            {
                SetStatus("Please enter a player nickname first.");
                return;
            }

            SaveAndEnterLobby(nicknameInput.text.Trim(), "Account");
        }

        private void SaveAndEnterLobby(string playerName, string method)
        {
            PlayerPrefs.SetString(PlayerNameKey, playerName);
            PlayerPrefs.SetString(LoginMethodKey, method);
            PlayerPrefs.Save();

            SetStatus($"Welcome {playerName}! Loading Lobby...");
            SceneManager.LoadScene(lobbySceneName);
        }

        private void SetStatus(string msg)
        {
            if (statusText != null)
            {
                statusText.text = msg;
            }
        }
    }
}
