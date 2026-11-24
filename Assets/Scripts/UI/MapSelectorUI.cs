using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleMobileIndia.Core;
using BattleMobileIndia.Data;

namespace BattleMobileIndia.UI
{
    public class MapSelectorUI : MonoBehaviour
    {
        [Header("Resources Path (without .json)")]
        public string configResourcePath = "Maps/regions_config";

        [Header("Dropdowns & Summary")]
        public Dropdown levelDropdown;    // 0: India, 1: State, 2: District
        public Dropdown stateDropdown;
        public Dropdown districtDropdown;
        public Text previewText;
        public Button confirmSelectionButton;

        private RegionMapCatalog _catalog;
        private List<StateEntry> _states = new List<StateEntry>();

        private void Awake()
        {
            LoadCatalog();
            SetupDropdowns();

            if (confirmSelectionButton != null)
            {
                confirmSelectionButton.onClick.AddListener(ApplySelection);
            }
        }

        private void LoadCatalog()
        {
            TextAsset asset = Resources.Load<TextAsset>(configResourcePath);
            if (asset != null && !string.IsNullOrEmpty(asset.text))
            {
                _catalog = JsonUtility.FromJson<RegionMapCatalog>(asset.text);
                _states = _catalog?.states ?? new List<StateEntry>();
            }
            else
            {
                _catalog = new RegionMapCatalog();
            }
        }

        private void SetupDropdowns()
        {
            if (levelDropdown != null)
            {
                levelDropdown.ClearOptions();
                levelDropdown.AddOptions(new List<string> { "India (100 Players)", "State Preset", "District Preset" });
                levelDropdown.onValueChanged.AddListener(_ => RefreshStateAndDistrictOptions());
            }

            if (stateDropdown != null)
            {
                stateDropdown.onValueChanged.AddListener(_ => RefreshDistrictOptions());
            }

            if (districtDropdown != null)
            {
                districtDropdown.onValueChanged.AddListener(_ => UpdatePreview());
            }

            RefreshStateAndDistrictOptions();
        }

        private void RefreshStateAndDistrictOptions()
        {
            if (stateDropdown != null)
            {
                stateDropdown.ClearOptions();
                List<string> stateNames = new List<string>();
                foreach (StateEntry st in _states)
                {
                    stateNames.Add(st.name);
                }

                if (stateNames.Count == 0)
                {
                    stateNames.Add("Rajasthan");
                }

                stateDropdown.AddOptions(stateNames);
            }

            RefreshDistrictOptions();
        }

        private void RefreshDistrictOptions()
        {
            if (districtDropdown == null)
            {
                UpdatePreview();
                return;
            }

            districtDropdown.ClearOptions();
            List<string> districtNames = new List<string>();

            int stateIdx = stateDropdown != null ? stateDropdown.value : 0;
            if (stateIdx >= 0 && stateIdx < _states.Count && _states[stateIdx].districts != null)
            {
                foreach (DistrictEntry dist in _states[stateIdx].districts)
                {
                    districtNames.Add(dist.name);
                }
            }

            if (districtNames.Count == 0)
            {
                districtNames.Add("Jaipur");
            }

            districtDropdown.AddOptions(districtNames);
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (previewText == null)
            {
                return;
            }

            int mode = levelDropdown != null ? levelDropdown.value : 0;
            string stateName = (stateDropdown != null && stateDropdown.options.Count > 0)
                ? stateDropdown.options[stateDropdown.value].text
                : "Rajasthan";
            string districtName = (districtDropdown != null && districtDropdown.options.Count > 0)
                ? districtDropdown.options[districtDropdown.value].text
                : "Jaipur";

            previewText.text = mode switch
            {
                1 => $"Selected: State - {stateName}",
                2 => $"Selected: District - {districtName} ({stateName})",
                _ => $"Selected: {_catalog?.indiaMain?.displayName ?? "BATTLE MOBILE INDIA - All India"}"
            };
        }

        public void ApplySelection()
        {
            int mode = levelDropdown != null ? levelDropdown.value : 0;
            int stateIdx = stateDropdown != null ? stateDropdown.value : 0;
            int distIdx = districtDropdown != null ? districtDropdown.value : 0;

            string label = "BATTLE MOBILE INDIA - All India";
            string sceneName = "03_Map_India_Main";

            if (mode == 1 && stateIdx >= 0 && stateIdx < _states.Count)
            {
                StateEntry st = _states[stateIdx];
                label = $"State: {st.name}";
                sceneName = string.IsNullOrEmpty(st.sceneName) ? "04_Map_State_Preset" : st.sceneName;
            }
            else if (mode == 2 && stateIdx >= 0 && stateIdx < _states.Count)
            {
                StateEntry st = _states[stateIdx];
                if (st.districts != null && distIdx >= 0 && distIdx < st.districts.Count)
                {
                    DistrictEntry dist = st.districts[distIdx];
                    label = $"District: {dist.name}, {st.name}";
                    sceneName = string.IsNullOrEmpty(dist.sceneName) ? "05_Map_District_Preset" : dist.sceneName;
                }
            }

            SceneFlowController.Instance?.SetSelectedRegion(label, sceneName);
            UpdatePreview();
        }
    }
}

