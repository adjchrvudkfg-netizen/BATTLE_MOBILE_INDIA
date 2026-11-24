#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattleMobileIndia.EditorTools
{
    public static class BMIProjectSettingsGenerator
    {
        [MenuItem("BMI Tools/Setup Tags, Physics & Build")]
        public static void SetupAll()
        {
            EnsureTagExists("LootPickup");
            ConfigureBuildScenes();
            AssetDatabase.SaveAssets();
            Debug.Log("BATTLE MOBILE INDIA project settings updated.");
        }

        private static void EnsureTagExists(string tagName)
        {
            SerializedObject tagManager = new SerializedObject(
                AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]
            );
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                if (tagsProp.GetArrayElementAtIndex(i).stringValue == tagName)
                {
                    return;
                }
            }

            tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
            tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1).stringValue = tagName;
            tagManager.ApplyModifiedProperties();
        }

        private static void ConfigureBuildScenes()
        {
            PlayerSettings.productName = "BATTLE MOBILE INDIA";

            string[] scenes =
            {
                "Assets/Scenes/01_Login.unity",
                "Assets/Scenes/02_Lobby.unity",
                "Assets/Scenes/03_Map_India_Main.unity",
                "Assets/Scenes/04_Map_State_Preset.unity",
                "Assets/Scenes/05_Map_District_Preset.unity"
            };

            List<EditorBuildSettingsScene> list = new List<EditorBuildSettingsScene>();
            foreach (string scenePath in scenes)
            {
                if (System.IO.File.Exists(scenePath))
                {
                    list.Add(new EditorBuildSettingsScene(scenePath, true));
                }
            }

            EditorBuildSettings.scenes = list.ToArray();
        }
    }
}
#endif

