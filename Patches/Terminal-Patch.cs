using System;
using HarmonyLib;
using UnityEngine;

namespace Pseudo_RealTerm.Patches;

[HarmonyPatch(typeof(Terminal))]

public class Terminal_Patch
{
    
    private static bool exitingTerminal = false;
    
    private static bool oldTerminal = false;

    private static TerminalNode TerminalNodeGenerator(string text, bool replace = true)
    {
        TerminalNode returnNode = ScriptableObject.CreateInstance<TerminalNode>();
        
        returnNode.displayText = text + "\n";
        returnNode.clearPreviousText = replace; // If it replaces the previous text
        
        return returnNode;
    }    

    [HarmonyPatch("BeginUsingTerminal")]
    [HarmonyPrefix]
    private static void BeginUsingTerminalPrefix(Terminal __instance)
    {
        oldTerminal = false; // Reset to custom terminal
    }
    
    
    
    [HarmonyPatch("OnSubmit")]
    [HarmonyPrefix]
    private static void OnSubmitPrefix(Terminal __instance)
    {
        
        string rawInputText =
            __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded); // This is copied from the decompiled source, wtf is this Zeekers
        
        Pseudo_RealTerm.Logger.LogMessage(rawInputText);

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
                string returnText = "Test text";
                bool replaceText = false;
                
                // Proof of concept:
                if (rawInputText == "old")
                {
                    returnText = "You are now using the vanilla terminal \n" +
                                 "exit the terminal to reset";
                    replaceText = true;
                    
                    oldTerminal = true;
                }
                
                __instance.LoadNewNode(TerminalNodeGenerator(returnText, replaceText));
                
                
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