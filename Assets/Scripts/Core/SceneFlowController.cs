using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleMobileIndia.Core
{
    public class SceneFlowController : MonoBehaviour
    {
        public static SceneFlowController Instance { get; private set; }

        [Header("Scene Names")]
        public string loginScene = "01_Login";
        public string lobbyScene = "02_Lobby";
        public string indiaMapScene = "03_Map_India_Main";
        public string stateMapScene = "04_Map_State_Preset";
        public string districtMapScene = "05_Map_District_Preset";

        public string SelectedSceneName { get; private set; }
        public string SelectedRegionLabel { get; private set; } = "BATTLE MOBILE INDIA - All India";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            SelectedSceneName = indiaMapScene;
        }

        public void LoadLogin()
        {
            SceneManager.LoadScene(loginScene);
        }

        public void LoadLobby()
        {
            SceneManager.LoadScene(lobbyScene);
        }

        public void SetSelectedRegion(string regionLabel, string sceneName)
        {
            SelectedRegionLabel = string.IsNullOrWhiteSpace(regionLabel)
                ? "BATTLE MOBILE INDIA - All India"
                : regionLabel;
            SelectedSceneName = string.IsNullOrWhiteSpace(sceneName)
                ? indiaMapScene
                : sceneName;
        }

        public void StartSelectedMatch()
        {
            string targetScene = string.IsNullOrWhiteSpace(SelectedSceneName)
                ? indiaMapScene
                : SelectedSceneName;
            SceneManager.LoadScene(targetScene);
        }
    }
}

