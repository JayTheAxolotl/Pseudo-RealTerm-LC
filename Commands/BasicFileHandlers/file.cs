using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Classes.FileSystem;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.BasicFileHandlers;

[TerminalCommand]
public static class fileCommand
{
    public static string Command = "file";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "file [options] [file]\n" +
                             "Options:\n" +
                             "--help : display this text";
        const bool replaceText = false;
        
        string returnText = "";

        if (line.Arguments.Count == 0)
        {
            returnText = "No file given\n" + usage;
        }
        else
        {
            Path usedPath = null;
            
            if (line.Arguments[0] == "/")
            {
                returnText = "That is not a file\n" + usage;
            }
            else if (Path.WouldBeAbsolute(line.Arguments[0]))
            {
                usedPath = new Path(line.Arguments[0]);
            }
            else
            {
                usedPath = TerminalPatch.WorkingPath.Append(line.Arguments[0]);
            }

            if (usedPath != null)
            {
                BaseFileType possibleFile = MasterDirectory.FileSystem.GetObjectFromPath(usedPath);
                
                if (possibleFile == null)
                {
                    returnText = "File does not exist or path is invalid";
                }
                else if (possibleFile is Directory)
                {
                    returnText = "That is a directory";
                }
                else
                {
                    File definiteFile = (File) possibleFile;
                    returnText = definiteFile.GetPath(true).ToString() + "\nFile type: " + definiteFile.Extension;
                }
            }
        }

        if (line.Options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}