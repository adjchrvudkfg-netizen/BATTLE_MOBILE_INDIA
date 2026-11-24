using System;
using System.Collections.Generic;
using UnityEngine;
using BattleMobileIndia.Core;
using BattleMobileIndia.Data;

namespace BattleMobileIndia.UI
{
    [Serializable]
    public class HudTargetElement
    {
        public string buttonId;
        public RectTransform rectTransform;
        public CanvasGroup canvasGroup;
    }

    [Serializable]
    public class HudLayoutSaveWrapper
    {
        public List<HudButtonLayoutItem> items = new List<HudButtonLayoutItem>();
    }

    public class CustomHudLayout : MonoBehaviour
    {
        private const string LayoutFileName = "bmi_hud_layout.json";

        public List<HudTargetElement> hudElements = new List<HudTargetElement>();

        private readonly Dictionary<string, Vector2> _defaultPositions = new Dictionary<string, Vector2>();
        private readonly Dictionary<string, Vector3> _defaultScales = new Dictionary<string, Vector3>();
        private readonly Dictionary<string, float> _defaultAlphas = new Dictionary<string, float>();

        private void Awake()
        {
            CacheDefaults();
            LoadLayout();
        }

        private void CacheDefaults()
        {
            foreach (var elem in hudElements)
            {
                if (elem == null || elem.rectTransform == null || string.IsNullOrEmpty(elem.buttonId))
                {
                    continue;
                }

                _defaultPositions[elem.buttonId] = elem.rectTransform.anchoredPosition;
                _defaultScales[elem.buttonId] = elem.rectTransform.localScale;
                _defaultAlphas[elem.buttonId] = elem.canvasGroup != null ? elem.canvasGroup.alpha : 1f;
            }
        }

        public void SaveLayout()
        {
            HudLayoutSaveWrapper wrapper = new HudLayoutSaveWrapper();

            foreach (var elem in hudElements)
            {
                if (elem == null || elem.rectTransform == null || string.IsNullOrEmpty(elem.buttonId))
                {
                    continue;
                }

                wrapper.items.Add(new HudButtonLayoutItem
                {
                    buttonId = elem.buttonId,
                    posX = elem.rectTransform.anchoredPosition.x,
                    posY = elem.rectTransform.anchoredPosition.y,
                    scale = elem.rectTransform.localScale.x,
                    alpha = elem.canvasGroup != null ? elem.canvasGroup.alpha : 1f
                });
            }

            SaveSystem.SaveJson(LayoutFileName, wrapper);
        }

        public void LoadLayout()
        {
            if (!SaveSystem.TryLoadJson(LayoutFileName, out HudLayoutSaveWrapper wrapper) || wrapper.items == null)
            {
                return;
            }

            Dictionary<string, HudButtonLayoutItem> map = new Dictionary<string, HudButtonLayoutItem>();
            foreach (var item in wrapper.items)
            {
                if (!string.IsNullOrEmpty(item.buttonId))
                {
                    map[item.buttonId] = item;
                }
            }

            foreach (var elem in hudElements)
            {
                if (elem == null || elem.rectTransform == null || string.IsNullOrEmpty(elem.buttonId))
                {
                    continue;
                }

                if (map.TryGetValue(elem.buttonId, out HudButtonLayoutItem saved))
                {
                    elem.rectTransform.anchoredPosition = new Vector2(saved.posX, saved.posY);
                    float s = Mathf.Clamp(saved.scale, 0.5f, 2.0f);
                    elem.rectTransform.localScale = new Vector3(s, s, 1f);
                    if (elem.canvasGroup != null)
                    {
                        elem.canvasGroup.alpha = Mathf.Clamp01(saved.alpha);
                    }
                }
            }
        }

        public void ResetToDefault()
        {
            foreach (var elem in hudElements)
            {
                if (elem == null || elem.rectTransform == null || string.IsNullOrEmpty(elem.buttonId))
                {
                    continue;
                }

                if (_defaultPositions.TryGetValue(elem.buttonId, out Vector2 pos))
                {
                    elem.rectTransform.anchoredPosition = pos;
                }

                if (_defaultScales.TryGetValue(elem.buttonId, out Vector3 scl))
                {
                    elem.rectTransform.localScale = scl;
                }

                if (elem.canvasGroup != null && _defaultAlphas.TryGetValue(elem.buttonId, out float alp))
                {
                    elem.canvasGroup.alpha = alp;
                }
            }

            SaveLayout();
        }
    }
}

