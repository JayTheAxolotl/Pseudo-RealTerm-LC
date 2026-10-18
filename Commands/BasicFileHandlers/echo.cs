using HarmonyLib;
using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.BasicFileHandlers;

[TerminalCommand]
public static class echo
{
    public static string Command = "echo";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "echo [options] [arguments]\n" +
                "Options:\n" +
                "--help : display this text";
        const bool replaceText = false;
        
        string returnText;
                    
        if (line.arguments.Count > 0)
        { returnText = line.arguments.Join(delimiter: " ") + "\n"; }
        else
        { returnText = "No arguments given\n" + usage; }

        if (line.options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}