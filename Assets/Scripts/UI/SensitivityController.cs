using System;
using UnityEngine;

namespace BattleMobileIndia.UI
{
    [Serializable]
    public class SensitivitySettings
    {
        public float general = 85f;
        public float redDot = 75f;
        public float scope2x = 65f;
        public float scope4x = 55f;
        public float sniperScope = 40f;
    }

    public class SensitivityController : MonoBehaviour
    {
        private const string PrefsKey = "BMI_Sensitivity_JSON";

        public SensitivitySettings Current { get; private set; } = new SensitivitySettings();

        private void Awake()
        {
            LoadSensitivity();
        }

        public void LoadSensitivity()
        {
            if (PlayerPrefs.HasKey(PrefsKey))
            {
                string json = PlayerPrefs.GetString(PrefsKey);
                if (!string.IsNullOrEmpty(json))
                {
                    Current = JsonUtility.FromJson<SensitivitySettings>(json);
                }
            }
        }

        public void SaveSensitivity()
        {
            string json = JsonUtility.ToJson(Current);
            PlayerPrefs.SetString(PrefsKey, json);
            PlayerPrefs.Save();
        }

        public void SetGeneral(float value) => Current.general = Mathf.Clamp(value, 1f, 200f);
        public void SetRedDot(float value) => Current.redDot = Mathf.Clamp(value, 1f, 200f);
        public void SetScope2x(float value) => Current.scope2x = Mathf.Clamp(value, 1f, 200f);
        public void SetScope4x(float value) => Current.scope4x = Mathf.Clamp(value, 1f, 200f);
        public void SetSniperScope(float value) => Current.sniperScope = Mathf.Clamp(value, 1f, 200f);

        public float GetActiveSensitivity(string scopeMode)
        {
            return scopeMode switch
            {
                "RedDot" => Current.redDot,
                "2x" => Current.scope2x,
                "4x" => Current.scope4x,
                "Sniper" => Current.sniperScope,
                _ => Current.general
            };
        }
    }
}

