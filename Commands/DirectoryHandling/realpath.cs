using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.DirectoryHandling;

[TerminalCommand]
public static class realpath
{
    public static string Command = "realpath";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "realpath [options] [arguments]\n" +
                             "Options:\n" +
                             "--help : display this text";
        const bool replaceText = false;

        string returnText = line.LineDirectory.ToString();

        if (line.options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}