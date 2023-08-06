using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EverChat 
{
    public class SettingsManager
    {
        private JObject settingsObject;
        private readonly string settingsFilePath;

        public SettingsManager()
        {
            settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EverChat", "settings.json");

            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(settingsFilePath))
                {
                    string json = File.ReadAllText(settingsFilePath);
                    settingsObject = JObject.Parse(json);
                }
                else
                {
                    settingsObject = new JObject();
                    InitializeDefaultSettings();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions during settings loading
                Console.WriteLine("An error occurred while loading settings: " + ex.Message);
            }
        }

        private void InitializeDefaultSettings()
        {
            settingsObject["StartInvisible"] = true;
            settingsObject["Notify"] = true;
        }

        public T GetSetting<T>(string key, T defaultValue = default)
        {
            if (settingsObject.ContainsKey(key))
            {
                return settingsObject[key].ToObject<T>();
            }

            return defaultValue;
        }

        public void SetSetting<T>(string key, T value)
        {
            settingsObject[key] = JToken.FromObject(value);
        }

        public void SaveSettings()
        {
            try
            {
                string settingsJson = settingsObject.ToString();
                Directory.CreateDirectory(Path.GetDirectoryName(settingsFilePath));
                File.WriteAllText(settingsFilePath, settingsJson);
            }
            catch (Exception ex)
            {
                // Handle exceptions during settings saving
                Console.WriteLine("An error occurred while saving settings: " + ex.Message);
            }
        }
    }
}
