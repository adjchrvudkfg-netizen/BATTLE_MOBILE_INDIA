using UnityEngine;
using UnityEngine.UI;

namespace BattleMobileIndia.UI
{
    public class SettingsUI : MonoBehaviour
    {
        [Header("Controllers")]
        public SensitivityController sensitivityController;
        public CustomHudLayout customHudLayout;

        [Header("Sensitivity Sliders")]
        public Slider generalSlider;
        public Slider redDotSlider;
        public Slider scope2xSlider;
        public Slider scope4xSlider;
        public Slider sniperSlider;

        [Header("Buttons")]
        public Button saveButton;
        public Button resetHudButton;

        private void Start()
        {
            SyncSlidersFromController();

            if (generalSlider != null) generalSlider.onValueChanged.AddListener(v => sensitivityController?.SetGeneral(v));
            if (redDotSlider != null) redDotSlider.onValueChanged.AddListener(v => sensitivityController?.SetRedDot(v));
            if (scope2xSlider != null) scope2xSlider.onValueChanged.AddListener(v => sensitivityController?.SetScope2x(v));
            if (scope4xSlider != null) scope4xSlider.onValueChanged.AddListener(v => sensitivityController?.SetScope4x(v));
            if (sniperSlider != null) sniperSlider.onValueChanged.AddListener(v => sensitivityController?.SetSniperScope(v));

            if (saveButton != null) saveButton.onClick.AddListener(SaveAllSettings);
            if (resetHudButton != null) resetHudButton.onClick.AddListener(ResetHud);
        }

        private void SyncSlidersFromController()
        {
            if (sensitivityController == null) return;
            var s = sensitivityController.Current;

            if (generalSlider != null) generalSlider.value = s.general;
            if (redDotSlider != null) redDotSlider.value = s.redDot;
            if (scope2xSlider != null) scope2xSlider.value = s.scope2x;
            if (scope4xSlider != null) scope4xSlider.value = s.scope4x;
            if (sniperSlider != null) sniperSlider.value = s.sniperScope;
        }

        public void SaveAllSettings()
        {
            sensitivityController?.SaveSensitivity();
            customHudLayout?.SaveLayout();
        }

        public void ResetHud()
        {
            customHudLayout?.ResetToDefault();
        }
    }
}

