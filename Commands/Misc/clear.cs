using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.Misc;

[TerminalCommand]
public static class clear
{
    public static string Command = "clear";
    
    public static void Run(Terminal instance, TermLine line)
    {

        const bool replaceText = true;
        
        const string returnText = "";
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}