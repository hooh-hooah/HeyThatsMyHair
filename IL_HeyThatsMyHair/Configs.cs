using BepInEx.Configuration;

namespace HeyThatsMyHairComponents
{
    public static class Configs
    {
        public static ConfigEntry<bool> Enabled { get; set; }
        public static ConfigEntry<bool> MergeAccessories { get; set; }

        private const string configName = "ThatsMyHair";
        public static void SetupConfigs(ConfigFile config)
        {
            Enabled = config.Bind(configName, "Enabled", true, new ConfigDescription("Enable this plugin"));
            MergeAccessories = config.Bind(configName, "Merge Accessories", false,
                new ConfigDescription("If coordinate has accessories in hair accessory slot, merge them not replacing them."));
        }
    }
}