using System.IO;
using UnityEngine;

namespace BattleMobileIndia.Core
{
    public static class SaveSystem
    {
        public static void SaveJson<T>(string fileName, T data)
        {
            string path = GetPath(fileName);
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }

        public static bool TryLoadJson<T>(string fileName, out T data) where T : new()
        {
            string path = GetPath(fileName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                if (!string.IsNullOrEmpty(json))
                {
                    data = JsonUtility.FromJson<T>(json);
                    return true;
                }
            }

            data = new T();
            return false;
        }

        private static string GetPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName);
        }
    }
}

