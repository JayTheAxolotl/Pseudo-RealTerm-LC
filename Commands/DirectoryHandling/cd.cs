using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.DirectoryHandling;

[TerminalCommand]
public static class cd
{
    public static string Command = "cd";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "cd [options] [arguments]\n" +
                             "Options:\n" +
                             "--help : display this text";
        const bool replaceText = false;
        
        string returnText = "";
                    
        if (line.arguments.Count == 0)
        {
            returnText = "No arguments found\n" + usage;
        }
        else
        {
            Directory targetDir;
            if (line.arguments[0].StartsWith("/")) // If it's absolute
            {
                targetDir = DirectoryHandler.StringToDirectory(line.arguments[0]);
            }
            else // If it isn't absolute, make it
            {
                targetDir = DirectoryHandler.StringToDirectory(line.LineDirectory.ToString() + line.arguments[0]);
            }
            
            DirectoryHandler.LoadDirs();
            if (DirectoryHandler.DirectoryExists(targetDir))
            {
                TerminalPatch.CurrentDirectory = targetDir;
            }
            else
            {
                returnText = "That directory does not exist";
            }
        }

        if (line.options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}