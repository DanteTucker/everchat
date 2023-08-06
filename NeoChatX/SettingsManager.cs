using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Forms;

namespace EverChat // Replace with your namespace
{
    public class SettingsManager
    {
        private Dictionary<string, object> settingsDictionary;

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
                    settingsDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                }
                else
                {
                    settingsDictionary = new Dictionary<string, object>();
                    InitializeDefaultSettings();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions during settings loading
                MessageBox.Show("An error occurred while loading settings: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDefaultSettings()
        {
            // Initialize default settings here
            settingsDictionary["StartInvisible"] = true;
            settingsDictionary["Notify"] = true;
        }

        public T GetSetting<T>(string key, T defaultValue = default)
        {
            if (settingsDictionary.ContainsKey(key))
            {
                return (T)settingsDictionary[key];
            }

            return defaultValue;
        }

        public void SetSetting<T>(string key, T value)
        {
            settingsDictionary[key] = value;
        }

        public void SaveSettings()
        {
            try
            {
                string settingsJson = JsonSerializer.Serialize(settingsDictionary);
                Directory.CreateDirectory(Path.GetDirectoryName(settingsFilePath));
                File.WriteAllText(settingsFilePath, settingsJson, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                // Handle exceptions during settings saving
                MessageBox.Show("An error occurred while saving settings: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
