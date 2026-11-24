using UnityEngine;
using UnityEngine.UI;
using BattleMobileIndia.Core;

namespace BattleMobileIndia.UI
{
    public class LobbyUI : MonoBehaviour
    {
        [Header("UI Panels & Texts")]
        public Text playerInfoText;
        public Text selectedMapText;
        public GameObject settingsPanel;
        public GameObject mapSelectorPanel;

        [Header("Buttons")]
        public Button playButton;
        public Button openSettingsButton;
        public Button openMapSelectorButton;

        private void Start()
        {
            if (GameManager.Instance != null && playerInfoText != null)
            {
                playerInfoText.text = $"{GameManager.Instance.ActivePlayerName} | {GameManager.Instance.ActivePlayerId}";
            }

            RefreshSelectedMapLabel();

            if (playButton != null)
            {
                playButton.onClick.AddListener(OnPlayClicked);
            }

            if (openSettingsButton != null)
            {
                openSettingsButton.onClick.AddListener(ToggleSettings);
            }

            if (openMapSelectorButton != null)
            {
                openMapSelectorButton.onClick.AddListener(ToggleMapSelector);
            }
        }

        public void RefreshSelectedMapLabel()
        {
            if (selectedMapText != null && SceneFlowController.Instance != null)
            {
                selectedMapText.text = $"Map: {SceneFlowController.Instance.SelectedRegionLabel}";
            }
        }

        public void ToggleSettings()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(!settingsPanel.activeSelf);
            }
        }

        public void ToggleMapSelector()
        {
            RefreshSelectedMapLabel();
            if (mapSelectorPanel != null)
            {
                mapSelectorPanel.SetActive(!mapSelectorPanel.activeSelf);
            }
        }

        private void OnPlayClicked()
        {
            GameManager.Instance?.SetGameState(GameState.PreMatchCountdown);
            SceneFlowController.Instance?.StartSelectedMatch();
        }
    }
}

