using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.Misc;

[TerminalCommand]
public static class help
{
    public static string Command = "help";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "old                        : switch to vanilla terminal\n" +
                             "echo [options] [arguments] : repeat what is told to it\n" +
                             "clear                      : clear all text\n" +
                             "di                         : Debug interact with the filesystem";
        const bool replaceText = false;

        string returnText = usage;
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}