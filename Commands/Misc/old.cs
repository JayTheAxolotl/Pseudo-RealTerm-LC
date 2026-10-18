using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.Misc;

[TerminalCommand]
public static class old
{
    public static string Command = "old";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string returnText = "You are now using the vanilla terminal \n" +
                                  "exit the terminal to reset";
        
        const bool replaceText = true;
        
        TerminalPatch.OldTerminal = true;
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}