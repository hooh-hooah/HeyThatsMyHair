using BepInEx;
using BepInEx.Harmony;
using HeyThatsMyHairComponents;

// Has dependency on moreaccessories by jaon6694
[BepInPlugin(GUID, "AI_HeyThatsMyHair", VERSION)]
[BepInDependency("com.bepis.bepinex.extendedsave")]
[BepInDependency("com.bepis.bepinex.sideloader")]
[BepInDependency("com.joan6694.illusionplugins.moreaccessories")]
public class HeyThatsMyHairPlugin : BaseUnityPlugin
{
    public const string GUID = "com.hooh.heythatsmyhair";
    public const string VERSION = "1.0.0";

    private void Start()
    {
        Hooks.Logger = Logger;
        HarmonyWrapper.PatchAll(typeof(Hooks));
        Configs.SetupConfigs(Config);
    }
}