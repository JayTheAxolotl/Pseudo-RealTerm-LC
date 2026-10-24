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
        const string usage = "echo [Options] [Arguments]\n" +
                "Options:\n" +
                "--help    : display this text\n" +
                "--options : return all given options, valid or not";
        const bool replaceText = false;
        
        string returnText;
        
        if (line.Arguments.Count > 0)
        { returnText = line.Arguments.Join(delimiter: " ") + "\n"; }
        else
        { returnText = "No Arguments given\n" + usage; }
        
        if (line.Options.Contains("--options"))
        {
            returnText = "";
            foreach (var option in line.Options) { returnText += option + "\n"; }
        }

        if (line.Options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}