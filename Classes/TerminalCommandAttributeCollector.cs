using System;
using System.Collections.Generic;
using LethalModDataLib.Helpers;
using Pseudo_RealTerm.Attributes;

namespace Pseudo_RealTerm.Classes;

public class TerminalCommandAttributeCollector
{
    public static List<Type> Commands = [];
    
    // Tries to register all classes in the mod as TerminalCommands
    internal static void RegisterTerminalLineAttributes()
    {
        foreach (Type loadableType in Pseudo_RealTerm.Instance.GetType().Assembly.GetLoadableTypes())
        {
            TerminalCommandAttributeCollector.RegisterTerminalLineAttributes(MyPluginInfo.PLUGIN_GUID, loadableType);
        }
    }
    
    /// <summary>
    ///     Registers all fields and properties decorated with ModData attributes in the given type.
    /// </summary>
    /// <param name="guid"> GUID of the plugin that registered the fields. </param>
    /// <param name="type"> Type to register the fields from. </param>
    /// <param name="instance"> Instance of the class to register the fields from. </param>
    /// <param name="keySuffix"> Suffix to append to the key. </param>
    internal static void RegisterTerminalLineAttributes(
        string guid,
        Type type,
        object? instance = null,
        string? keySuffix = null)
    {
        try
        {
            TerminalCommandAttribute commandAttribute = (TerminalCommandAttribute)Attribute.GetCustomAttribute(type, typeof(TerminalCommandAttribute));
            if (commandAttribute != null)
            {
                Pseudo_RealTerm.Logger.LogMessage($"Registering {type} with {guid}");
                Commands.Add(type);
            }
        }
        catch (Exception ex)
        {
            Pseudo_RealTerm.Logger?.LogError((object) $"Failed to register TerminalCommand attributes in {type.FullName} from {guid} plugin: {ex.Message}");
        }
    }
}