using HarmonyLib;
using Pseudo_RealTerm.Classes;
using UnityEngine;

namespace Pseudo_RealTerm.Patches;

[HarmonyPatch(typeof(Terminal))]

public class TerminalPatch
{
    
    private static bool exitingTerminal = false;
    
    public static bool oldTerminal = false;
    
    private static string currentDirectory = "/";

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
        oldTerminal = false; // Reset to custom terminal
        
        currentDirectory = "/";
        
        WriteToTerminal("PseudoS v0.1\n" + 
                        "Type \"help\" for help", __instance);
    }
    
    
    
    [HarmonyPatch("OnSubmit")]
    [HarmonyPrefix]
    private static void OnSubmitPrefix(Terminal __instance)
    {
        
        string rawInputText =
            __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded); // This is copied from the decompiled source, wtf is this Zeekers

        if (!oldTerminal && 
            !exitingTerminal) // [ExitTerm]
        {
            __instance.terminalInUse = false; // Bypasses vanilla functionality, reset in postfix
            if (rawInputText.Replace(" ", "").Length == 0)
            {
                __instance.screenText.ActivateInputField();
                __instance.screenText.Select();
            }
            else // Main functionality 
            {
                TermLine line = new TermLine(rawInputText);
                line.Directory = currentDirectory;
                
                foreach (var command in TerminalCommandAttributeCollector.Commands) // For each command that was collected
                {
                    Pseudo_RealTerm.Logger.LogMessage(command);
                    if ((string) command.GetField("Command").GetValue(null) // equivalent to old.Command == line.Command and etc
                        == line.Command)
                    { command.GetMethod("Run")?.Invoke(null, [__instance, line]); } // equivalent to old.Run(__instance, line) and etc
                }
                
                
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
        if (!exitingTerminal)
        {
            __instance.terminalInUse = true;
        }
        else
        {   // [ExitTerm]
            __instance.terminalInUse = false;
            exitingTerminal = false;
        }
    }
    
    
    
    // Exiting the terminal is weird, so this is to bypass my mod and do the vanilla stuff
    // "[ExitTerm]" means stuff related to this
    [HarmonyPatch("QuitTerminal")]
    [HarmonyPrefix]
    private static void QuitTerminalPrefix(Terminal __instance)
    {
        exitingTerminal = true;
    }
    
}