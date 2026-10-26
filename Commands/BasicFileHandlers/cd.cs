using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Classes.FileSystem;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.BasicFileHandlers;

[TerminalCommand]
public static class cd
{
    public static string Command = "cd";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "cd [options] [path]\n" +
                             "Options:\n" +
                             "--help : display this text";
        const bool replaceText = false;
        
        string returnText;

        if (line.Arguments.Count == 0)
        {
            returnText = "/";
            TerminalPatch.WorkingPath = new Path("/");
        }
        else
        {
            Path newPath;
            if (Path.WouldBeAbsolute(line.Arguments[0]))
            { newPath = new Path(line.Arguments[0]); }
            else
            { newPath = TerminalPatch.WorkingPath.Append(line.Arguments[0]); }

            if (MasterDirectory.FileSystem.GetDirectoryFromPath(newPath) != null)
            {
                returnText = newPath.PathString;
                TerminalPatch.WorkingPath = newPath;
            }
            else
            {
                returnText = "That directory does not exist";
            }
        }

        if (line.Options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}