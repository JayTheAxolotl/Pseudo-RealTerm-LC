using System.Runtime.CompilerServices;
using HarmonyLib;
using LethalModDataLib.Features;
using LethalModDataLib.Helpers;
using Pseudo_RealTerm.Classes;
using UnityEngine;

namespace Pseudo_RealTerm.Patches;

[HarmonyPatch(typeof(Terminal))]

public class TerminalPatch
{
    
    private static bool exitingTerminal = false;
    
    private static bool oldTerminal = false;

    private static string currentDirectory = "/";

    private static TerminalNode WriteToTerminal(string text, string lastText, bool replace = true)
    {
        TerminalNode returnNode = ScriptableObject.CreateInstance<TerminalNode>();

        if (replace)
        { returnNode.displayText = text + "\n"; }
        else
        { returnNode.displayText = lastText.Replace("\n\n\n", "") + "\n" + text + "\n"; }
        
        returnNode.clearPreviousText = true; // make it always replaces the previous text, letting me control replacing better
        
        return returnNode;
    }    
    
    
    
    [HarmonyPatch("BeginUsingTerminal")]
    [HarmonyPostfix]
    private static void BeginUsingTerminalPostfix(Terminal __instance)
    {
        oldTerminal = false; // Reset to custom terminal
        
        currentDirectory = "/";
        
        __instance.LoadNewNode(WriteToTerminal("PseudoS v0.1\n" +
                                               "Type \"help\" for help",
            __instance.screenText.text));
        
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
                string returnText = "";
                bool replaceText = false;
                termLine line = new termLine(rawInputText);
                string usage = "";
                
                
                if (line.command == "old")
                {
                    returnText = "You are now using the vanilla terminal \n" +
                                 "exit the terminal to reset";
                    replaceText = true;
                    
                    oldTerminal = true;
                } 
                else if (line.command == "echo")
                {
                    usage = "echo [options] [arguments]\n" +
                            "Options:\n" +
                            "--help : display this text";
                    
                    if (line.arguments.Count > 0)
                    { returnText = line.arguments.Join(delimiter: " "); }
                    else
                    { returnText = "No arguments given\n" + usage; }
                    
                } 
                else if (line.command == "clear")
                {
                    returnText = "";
                    replaceText = true;
                } 
                else if (line.command == "help")
                {
                    usage = "old                        : switch to vanilla terminal\n" +
                            "echo [options] [arguments] : repeat what is told to it\n" +
                            "clear                      : clear the terminal";
                    returnText = usage;
                }
                else if (line.command == "touch")
                {
                    usage = "touch [options] [filename]\n"+ 
                            "Options:\n" + 
                            "--help : display this text";

                    SaveLoadHandler.LoadData(ModDataHelper.GetModDataKey(
                        typeof(FileHandler), 
                        nameof(FileHandler.FileList)));

                    if (line.arguments.Count > 0)
                    {
                        string[] dirAndName = FileHandler.FileNameToPath(line.arguments[0]);
                        if (!dirAndName[1].StartsWith("/")) {dirAndName[1] = currentDirectory + dirAndName[1];} // Relative path if absolute is not given
                        
                        returnText = "made file: \"" + dirAndName[0] + "\" at \"" + dirAndName[1] + "\"" ;
                        
                        File newFile = new File();
                        newFile.Name = dirAndName[0];
                        newFile.Path = dirAndName[1];
                        FileHandler.FileList.Add(newFile);
                        
                        SaveLoadHandler.SaveData(ModDataHelper.GetModDataKey(
                            typeof(FileHandler), 
                            nameof(FileHandler.FileList)));
                        
                    }
                    else
                    { returnText = "No arguments given\n" + usage; }
                }
                else if (line.command == "ls")
                {
                    usage = "ls [options] [dir]\n"+ 
                            "Options:\n" + 
                            "--help : display this text";
                    
                    SaveLoadHandler.LoadData(ModDataHelper.GetModDataKey(
                        typeof(FileHandler), 
                        nameof(FileHandler.FileList)));
                    
                    returnText = "";
                    foreach (var file in FileHandler.FileList)
                    {
                        returnText += file.Name + "   ";
                    }
                }
                
                if (line.options.Contains("--help")) { returnText = usage; } // help text
                __instance.LoadNewNode(WriteToTerminal(returnText, __instance.screenText.text, replaceText));
                
                
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