using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace BattleMobileIndia.UI
{
    public class FreeFireLobbyUI : MonoBehaviour
    {
        [Header("Player Profile HUD")]
        public TMP_Text playerNameText;
        public TMP_Text playerUidText;
        public TMP_Text selectedMapText;

        [Header("Map Quick Select Buttons")]
        public Button indiaMapButton;
        public Button stateMapButton;
        public Button districtMapButton;

        [Header("Start Match Button")]
        public Button startMatchButton;
        public TMP_Text startButtonLabel;

        [Header("Default Scene Names")]
        public string indiaSceneName = "03_Map_India_Main";
        public string stateSceneName = "04_Map_State_Preset";
        public string districtSceneName = "05_Map_District_Preset";

        private string currentSceneToLoad;

        private void Start()
        {
            string pName = PlayerPrefs.GetString("BMI_PlayerName", "BMI_Survivor");
            string pUid = PlayerPrefs.GetString("BMI_PlayerUID", "");

            if (string.IsNullOrEmpty(pUid))
            {
                pUid = "UID: " + Random.Range(10000000, 99999999);
                PlayerPrefs.SetString("BMI_PlayerUID", pUid);
                PlayerPrefs.Save();
            }

            if (playerNameText != null) playerNameText.text = pName;
            if (playerUidText != null) playerUidText.text = pUid;

            if (indiaMapButton != null)
                indiaMapButton.onClick.AddListener(() => SelectMap("MAP: INDIA (100P)", indiaSceneName));

            if (stateMapButton != null)
                stateMapButton.onClick.AddListener(() => SelectMap("MAP: STATE PRESET", stateSceneName));

            if (districtMapButton != null)
                districtMapButton.onClick.AddListener(() => SelectMap("MAP: DISTRICT PRESET", districtSceneName));

            if (startMatchButton != null)
                startMatchButton.onClick.AddListener(OnStartMatchClicked);

            SelectMap("MAP: INDIA (100P)", indiaSceneName);
        }

        public void SelectMap(string label, string sceneName)
        {
            currentSceneToLoad = sceneName;
            PlayerPrefs.SetString("BMI_SelectedScene", sceneName);
            PlayerPrefs.Save();

            if (selectedMapText != null)
            {
                selectedMapText.text = label;
            }
        }

        public void OnStartMatchClicked()
        {
            string targetScene = string.IsNullOrEmpty(currentSceneToLoad) ? indiaSceneName : currentSceneToLoad;
            if (startButtonLabel != null)
            {
                startButtonLabel.text = "MATCHMAKING...";
            }
            SceneManager.LoadScene(targetScene);
        }
    }
}
