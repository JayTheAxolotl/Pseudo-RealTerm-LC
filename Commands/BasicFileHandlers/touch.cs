using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Classes.FileSystem;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands;

[TerminalCommand]
public static class touch
{
    public static string Command = "touch";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "touch [options] [arguments]\n" +
                             "Options:\n" +
                             "--help : display this text";
        const bool replaceText = false;
        
        string returnText;

        if (line.Arguments.Count == 0)
        {
            returnText = "No arguments given \n" + usage;
        }
        else
        {
            Path fullPath;
            if (Path.WouldBeAbsolute(line.Arguments[0]))
            {
                fullPath = new Path(line.Arguments[0]);
            }
            else
            {
                fullPath = TerminalPatch.WorkingPath.Append(line.Arguments[0]);
            }

            Path writePath = fullPath.Pop();
            File newFile = new File(fullPath.PathList[^1]);
            
            // If the write path doesn't exist
            if (MasterDirectory.FileSystem.GetDirectoryFromPath(writePath) == null && writePath.PathString != "/")
            {
                returnText = "Write directory not found";
            }
            // If the file already exists, or a directory with its name
            else if (MasterDirectory.FileSystem.GetObjectFromPath(fullPath) != null)
            {
                returnText = "That file already exists";
            }
            else
            {
                MasterDirectory.AddFileToSystem(newFile, writePath);
                returnText = fullPath.ToString();
            }
        }

        if (line.Options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}