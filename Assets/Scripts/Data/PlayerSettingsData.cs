using System;

namespace BattleMobileIndia.Data
{
    [Serializable]
    public class HudButtonLayoutItem
    {
        public string buttonId;
        public float posX;
        public float posY;
        public float scale = 1f;
        public float alpha = 0.85f;
    }

    [Serializable]
    public class PlayerSettingsData
    {
        public float generalSensitivity = 85f;
        public float redDotSensitivity = 75f;
        public float scope2xSensitivity = 65f;
        public float scope4xSensitivity = 55f;
        public float sniperSensitivity = 40f;
        public int graphicsQualityIndex = 1;
        public float masterVolume = 0.8f;
        public HudButtonLayoutItem[] hudButtons = Array.Empty<HudButtonLayoutItem>();
    }
}

