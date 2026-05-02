using System;
using System.IO;
using System.Text.Json;

namespace PharmoSys.Services
{
    public class StoreSettings
    {
        public string StoreName { get; set; } = "PharmoSys Default Store";
        public string StoreAddress { get; set; } = "123 Main Street, City";
        public string StoreContact { get; set; } = "0300-1234567";
    }

    public static class SettingsService
    {
        private static readonly string SettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public static StoreSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    var json = File.ReadAllText(SettingsFile);
                    return JsonSerializer.Deserialize<StoreSettings>(json) ?? new StoreSettings();
                }
            }
            catch
            {
                // Return defaults on failure
            }
            return new StoreSettings();
        }

        public static void SaveSettings(StoreSettings settings)
        {
            try
            {
                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFile, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
    }
}
