using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalModDataLib.Attributes;
using LethalModDataLib.Enums;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Commands;

namespace Pseudo_RealTerm;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(LethalModDataLib.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
public class Pseudo_RealTerm : BaseUnityPlugin
{
    public static Pseudo_RealTerm Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }
    
    [ModData(SaveWhen.OnSave, LoadWhen.OnLoad, SaveLocation.CurrentSave)]
    private static int _nextUID;
    
    /// Returns and iterates a unique identifier for Files and Directories.
    /// Saved between saves
    public static int NextUID()
    {
        return _nextUID++;
    }
    
    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        Patch();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        
        TerminalCommandAttributeCollector.RegisterTerminalLineAttributes();
    }

    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll();

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }
}