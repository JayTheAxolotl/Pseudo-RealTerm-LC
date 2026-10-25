using HarmonyLib;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Classes.FileSystem;
using UnityEngine;

namespace Pseudo_RealTerm.Patches;

[HarmonyPatch(typeof(Terminal))]

public class TerminalPatch
{
    
    private static bool _exitingTerminal = false;
    
    public static bool OldTerminal = false;

    public static Path CurrentPath;

    /// <summary>
    /// Writes to the terminal screen
    /// </summary>
    /// <param name="text">The text to write</param>
    /// <param name="instance">The terminal screen instance to write to</param>
    /// <param name="replace">If it replaces all text, or appends to it</param>
    public static void WriteToTerminal(string text, Terminal instance, bool replace = true)
    {
        TerminalNode returnNode = ScriptableObject.CreateInstance<TerminalNode>();

        if (replace)
        { returnNode.displayText = text + "\n"; }
        else
        { returnNode.displayText = instance.screenText.text.Replace("\n\n\n", "") + "\n" + text + "\n"; }
        
        returnNode.clearPreviousText = true; // make it always replaces the previous text, letting me control replacing better
        
        instance.LoadNewNode(returnNode);

    }    
    
    
    
    [HarmonyPatch("BeginUsingTerminal")]
    [HarmonyPostfix]
    private static void BeginUsingTerminalPostfix(Terminal __instance)
    {
        OldTerminal = false; // Reset to custom terminal
        
        WriteToTerminal("PseudoS v0.2\n" + 
                        "Type \"help\" for help", __instance);

        CurrentPath = new Path("/");
    }
    
    
    
    [HarmonyPatch("OnSubmit")]
    [HarmonyPrefix]
    private static void OnSubmitPrefix(Terminal __instance)
    {
        string rawInputText =
            __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded); // This is copied from the decompiled source, wtf is this Zeekers

        if (!OldTerminal && 
            !_exitingTerminal) // [ExitTerm]
        {
            __instance.terminalInUse = false; // Bypasses vanilla functionality, reset in postfix
            if (rawInputText.Replace(" ", "").Length == 0)
            {
                __instance.screenText.ActivateInputField();
                __instance.screenText.Select();
            }
            else // Main functionality 
            {
                TermLine line = new TermLine(rawInputText); // Parse the input
                bool commandRecognized = false;
                
                // Checks the line command against each collected command, and run it if it matches
                foreach (var command in TerminalCommandAttributeCollector.Commands) 
                {
                    // equivalent to old.Command == line.Command and etc
                    if ((string)command.GetField("Command").GetValue(null) == line.Command)
                    {
                        command.GetMethod("Run")?.Invoke(null, [__instance, line]); // equivalent to old.Run(__instance, line) and etc
                        commandRecognized = true;
                        break; // Slightly better for performance to break
                    } 
                }
                
                if (!commandRecognized) { WriteToTerminal(line.Command + " not recognized", __instance, false); }
                
                // Copied from source, resets everything
                __instance.modifyingText = true;
                __instance.screenText.text = __instance.screenText.text.Substring(0, __instance.screenText.text.Length - __instance.textAdded);
                __instance.currentText = __instance.screenText.text;
                __instance.textAdded = 0;
                
                __instance.screenText.ActivateInputField();
                __instance.screenText.Select();
            }
        }
    }

    [HarmonyPatch("OnSubmit")]
    [HarmonyPostfix]
    private static void OnSubmitPostfix(Terminal __instance)
    {   
        if (!_exitingTerminal)
        {
            __instance.terminalInUse = true;
        }
        else
        {   // [ExitTerm]
            __instance.terminalInUse = false;
            _exitingTerminal = false;
        }
    }
    
    
    
    // Exiting the terminal is weird, so this is to bypass my mod and do the vanilla stuff
    // "[ExitTerm]" means stuff related to this
    [HarmonyPatch("QuitTerminal")]
    [HarmonyPrefix]
    private static void QuitTerminalPrefix(Terminal __instance)
    {
        _exitingTerminal = true;
    }

    [HarmonyPatch("TextChanged")]
    [HarmonyPrefix]
    private static void TextChangedPrefix(Terminal __instance)
    {
        if ((UnityEngine.Object) __instance.currentNode == (UnityEngine.Object) null)
            return;
        __instance.currentNode.maxCharactersToType = 1024; // Virtually remove the character limit
    }
    
}