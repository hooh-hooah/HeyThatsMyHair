using System.Collections.Generic;
using System.Linq;
using AIChara;
using BepInEx.Logging;
using HarmonyLib;

namespace HeyThatsMyHairComponents
{
    public static class Hooks
    {
        // TODO: UI Integration
        // TODO: Append Accessories, Load Only Hairs.. etc.
        
        // well shit it's too complicated to remember all of shits so...
        private static Dictionary<int, HashSet<int>> knownHairItems = new Dictionary<int, HashSet<int>>();
        // You're not going to click coordinate load for like 10 times in a second, right?
        private static Dictionary<int, ChaFileAccessory.PartsInfo> partsCache = new Dictionary<int, ChaFileAccessory.PartsInfo>();
        public static ManualLogSource Logger { get; set; }

        [HarmonyPostfix, HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeSettingHairTypeAccessoryShader))]
        public static void RememberSlot(ChaControl __instance, int slotNo)
        {
            var cmpAccessory = __instance.cmpAccessory[slotNo];
            if (ReferenceEquals(null, cmpAccessory) || !cmpAccessory.typeHair) return;
            var part = __instance.nowCoordinate.accessory.parts[slotNo];

            if (!knownHairItems.ContainsKey(part.type)) knownHairItems[part.type] = new HashSet<int>();
            knownHairItems[part.type].Add(part.id);
        }

        // TODO: add option to turn this option off.
        // don't change first 20 accessory list.
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeNowCoordinate), typeof(string), typeof(bool), typeof(bool))]
        public static void KeepHairList(ChaControl __instance, string path)
        {
            if (Configs.Enabled.Value == false) return;
            partsCache.Clear();
            var coord = __instance.nowCoordinate;
            var coordCopy = coord.accessory.parts.ToArray(); // just to make sure.
            for (var index = 0; index < coord.accessory.parts.Length; index++)
            {
                var part = coord.accessory.parts[index];
                if (!knownHairItems.TryGetValue(part.type, out var hashSet) || !hashSet.Contains(part.id)) continue;
                partsCache[index] = coordCopy[index];
            }
        }

        // restore first 20 accessory list.
        [HarmonyPrefix]
        // ChangeNowCoordinate(ChaFileCoordinate srcCoorde, bool reload = false, bool forceChange = true)
        [HarmonyPatch(typeof(ChaControl), nameof(ChaControl.ChangeNowCoordinate), typeof(ChaFileCoordinate), typeof(bool), typeof(bool))]
        public static void RestoreHairList(ChaControl __instance, ChaFileCoordinate srcCoorde)
        {
            if (Configs.Enabled.Value == false) return;
            // Reserve Hair Slots
            foreach (KeyValuePair<int, ChaFileAccessory.PartsInfo> kv in partsCache)
            {
                srcCoorde.accessory.parts[kv.Key] = kv.Value;
            }
            partsCache.Clear();
        }
    }
}