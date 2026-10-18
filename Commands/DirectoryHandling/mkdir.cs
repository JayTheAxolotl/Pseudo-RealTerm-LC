using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.DirectoryHandling;

[TerminalCommand]
public static class mkdir
{
    public static string Command = "mkdir";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "mkdir [options] [arguments]\n" +
                             "Options:\n" +
                             "--help : display this text";
        const bool replaceText = false;
        
        string returnText;

        DirectoryHandler.LoadDirs();
        
        if (line.arguments.Count == 0)
        {
            returnText = "No arguments found\n" + usage;
        }
        else
        {
            Directory returnDirectory;
            if (line.arguments[0].StartsWith("/")) // If it's absolute
            { 
                returnDirectory = DirectoryHandler.StringToDirectory(line.arguments[0]);
            }
            else // If it isn't absolute, make it
            {
                returnDirectory = DirectoryHandler.StringToDirectory(line.LineDirectory.ToString() + line.arguments[0]);
            }

            if (DirectoryHandler.DirectoryExists(returnDirectory))
            {
                returnText = "That directory exists";
            }
            else
            {
                returnText = returnDirectory.ToString();
                DirectoryHandler.DirectoryList.Add(returnDirectory);
                DirectoryHandler.SaveDirs();
            }
        }

        if (line.options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}