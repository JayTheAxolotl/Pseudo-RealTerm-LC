using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Classes.FileSystem;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands;

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

        if (line.Arguments.Count == 0)
        {
            returnText = "No arguments given" + usage;
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
            Directory newDirectory = new Directory(fullPath.PathList[^1]);
            
            // If the write path doesn't exist
            if (MasterDirectory.FileSystem.GetDirectoryFromPath(writePath) == null && writePath.PathString != "/")
            {
                returnText = "Write directory not found";
            }
            // If the directory already exists, or a file with its name
            else if (MasterDirectory.FileSystem.GetObjectFromPath(fullPath) != null)
            {
                returnText = "That directory already exists";
            }
            else
            {
                MasterDirectory.AddFileToSystem(newDirectory, writePath);
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