using System;
using System.Collections.Generic;
using LethalModDataLib.Helpers;
using Pseudo_RealTerm.Attributes;

namespace Pseudo_RealTerm.Classes;

public static class TerminalCommandAttributeCollector
{
    public static List<Type> Commands = [];
    
    // Tries to register all classes in the mod as TerminalCommands
    internal static void RegisterTerminalLineAttributes()
    {
        foreach (Type loadableType in Pseudo_RealTerm.Instance.GetType().Assembly.GetLoadableTypes())
        {
            RegisterTerminalLineAttributes(MyPluginInfo.PLUGIN_GUID, loadableType);
        }
    }
    
    /// Registers all fields and properties decorated with TerminalCommandAttribute attributes in the given type.
    internal static void RegisterTerminalLineAttributes(
        string guid,
        Type type)
    {
        try
        {
            TerminalCommandAttribute commandAttribute = (TerminalCommandAttribute) Attribute.GetCustomAttribute(type, typeof(TerminalCommandAttribute));
            if (commandAttribute != null)
            {
                Pseudo_RealTerm.Logger.LogDebug($"Registering command {type}");
                Commands.Add(type);
            }
        }
        catch (Exception ex)
        {
            Pseudo_RealTerm.Logger?.LogError((object) $"Failed to register TerminalCommand attributes in {type.FullName} from {guid} plugin: {ex.Message}");
        }
    }
}