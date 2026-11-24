using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleMobileIndia.Core;

namespace BattleMobileIndia.Match
{
    public class MatchSpawner100 : MonoBehaviour
    {
        [Header("Match Count (1 Player + 99 Bots = 100)")]
        public int totalPlayersTarget = 100;
        public GameObject playerPrefab;
        public GameObject botPrefab;
        public Transform[] spawnPoints;

        [Header("UI & Countdown")]
        public Text aliveCountText;
        public PreMatchCountdown25s countdown25s;

        private readonly List<GameObject> _spawnedEntities = new List<GameObject>();

        private void Start()
        {
            SpawnMatchRoster();

            if (countdown25s != null)
            {
                countdown25s.OnCountdownFinished += HandleCountdownFinished;
                countdown25s.StartCountdown(25f);
            }
        }

        private void SpawnMatchRoster()
        {
            _spawnedEntities.Clear();
            int countToSpawn = Mathf.Max(1, totalPlayersTarget);

            for (int i = 0; i < countToSpawn; i++)
            {
                Vector3 pos = GetSpawnPosition(i);
                if (i == 0 && playerPrefab != null)
                {
                    GameObject p = Instantiate(playerPrefab, pos, Quaternion.identity);
                    _spawnedEntities.Add(p);
                }
                else if (botPrefab != null)
                {
                    GameObject b = Instantiate(botPrefab, pos, Quaternion.identity);
                    b.name = $"Bot_{i:000}";
                    _spawnedEntities.Add(b);
                }
            }

            UpdateAliveCountUI();
        }

        private Vector3 GetSpawnPosition(int index)
        {
            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                Transform pt = spawnPoints[index % spawnPoints.Length];
                Vector2 jitter = Random.insideUnitCircle * 4f;
                return pt.position + new Vector3(jitter.x, 0f, jitter.y);
            }

            Vector2 ring = Random.insideUnitCircle * 35f;
            return new Vector3(ring.x, 1f, ring.y);
        }

        private void HandleCountdownFinished()
        {
            GameManager.Instance?.SetGameState(GameState.InMatch);
        }

        public void UpdateAliveCountUI()
        {
            _spawnedEntities.RemoveAll(item => item == null);
            if (aliveCountText != null)
            {
                aliveCountText.text = $"Alive: {_spawnedEntities.Count}/{totalPlayersTarget}";
            }
        }

        private void OnDestroy()
        {
            if (countdown25s != null)
            {
                countdown25s.OnCountdownFinished -= HandleCountdownFinished;
            }
        }
    }
}

