using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.BasicFileHandlers;

[TerminalCommand]
public static class ls
{
    public static string Command = "ls";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "ls [options] [dir]\n"+ 
                             "Options:\n" + 
                             "--help : display this text";
        const bool replaceText = false;
        
        string returnText = "";

        if (line.arguments.Count > 0)
        {
            string[] splitArgumentsPath = FileHandler.SplitFullPath(line.arguments[0]);
            if (splitArgumentsPath[1] != "")
            {
                line.LineDirectory = new Directory();
            }
        }

        FileHandler.LoadFiles();
        foreach (var file in FileHandler.FileList)
        {
            if (file.Directory.ToString() == line.LineDirectory.ToString())
            {
                returnText += file.Name + "   ";
            }
        }
        
        DirectoryHandler.LoadDirs();
        foreach (var directory in DirectoryHandler.DirectoryList)
        {
            // Two of these because it removes both "/"s when in the root directory
            if (directory.ToString().Replace(line.LineDirectory.ToString(), "") == directory.Path[^1])
            {
                returnText += directory.ToString().Replace(line.LineDirectory.ToString(), "") + "/   ";
            }
            else if (directory.ToString().Replace(line.LineDirectory.ToString(), "") == directory.Path[^1] + "/")
            {
                returnText += directory.ToString().Replace(line.LineDirectory.ToString(), "") + "   ";
            }
        }

        if (line.options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}