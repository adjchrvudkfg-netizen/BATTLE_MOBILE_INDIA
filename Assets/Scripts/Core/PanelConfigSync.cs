using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using BattleMobileIndia.Match;
using BattleMobileIndia.UI;

namespace BattleMobileIndia.Core
{
    [Serializable]
    public class PanelSelectedRegion
    {
        public string level;
        public string state;
        public string district;
        public string sceneName;
    }

    [Serializable]
    public class PanelMatchConfig
    {
        public int maxPlayers = 100;
        public int countdownSeconds = 25;
        public PanelSelectedRegion selectedRegion = new PanelSelectedRegion();
    }

    [Serializable]
    public class PanelStateResponse
    {
        public string brandName;
        public PanelMatchConfig matchConfig = new PanelMatchConfig();
    }

    public class PanelConfigSync : MonoBehaviour
    {
        [Header("Panel API URL")]
        public string panelBaseUrl = "http://localhost:3000";

        [Header("Optional Target References")]
        public MatchSpawner100 matchSpawner;
        public PreMatchCountdown25s countdown25s;
        public LobbyUI lobbyUI;

        private void Start()
        {
            StartCoroutine(FetchPanelState());
        }

        public IEnumerator FetchPanelState()
        {
            string url = $"{panelBaseUrl.TrimEnd('/')}/api/state";
            using UnityWebRequest req = UnityWebRequest.Get(url);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"Panel sync failed: {req.error}");
                yield break;
            }

            PanelStateResponse data = JsonUtility.FromJson<PanelStateResponse>(req.downloadHandler.text);
            if (data == null || data.matchConfig == null)
            {
                yield break;
            }

            if (matchSpawner != null)
            {
                matchSpawner.totalPlayersTarget = Mathf.Clamp(data.matchConfig.maxPlayers, 1, 100);
            }

            if (countdown25s != null)
            {
                countdown25s.startSeconds = Mathf.Max(0f, data.matchConfig.countdownSeconds);
            }

            if (data.matchConfig.selectedRegion != null && SceneFlowController.Instance != null)
            {
                var r = data.matchConfig.selectedRegion;
                string label = r.level == "District"
                    ? $"District: {r.district}, {r.state}"
                    : (r.level == "State" ? $"State: {r.state}" : "BATTLE MOBILE INDIA - All India");

                SceneFlowController.Instance.SetSelectedRegion(label, r.sceneName);
                lobbyUI?.RefreshSelectedMapLabel();
            }
        }
    }
}

