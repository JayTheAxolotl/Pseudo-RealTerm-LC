using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Classes.FileSystem;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands;

[TerminalCommand]
public static class ls
{
    public static string Command = "ls";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "ls [options] [directory]\n" +
                             "Options:\n" +
                             "--help : display this text";
        const bool replaceText = false;
        
        string returnText = "";
        Path listedPath;

        if (line.Arguments.Count == 0)
        {
            listedPath = TerminalPatch.CurrentPath;
        }
        else
        {
            if (line.Arguments[0] == "/")
            {
                listedPath = new Path("/");
            }
            else if (Path.WouldBeAbsolute(line.Arguments[0]))
            {
                listedPath = new Path(line.Arguments[0]);
            }
            else
            {
                listedPath = TerminalPatch.CurrentPath.Append(line.Arguments[0]);
            }
        }
        
        Directory listedDirectory;
        
        if (listedPath.PathString == "/")
        { listedDirectory = MasterDirectory.FileSystem; }
        else
        { listedDirectory = MasterDirectory.FileSystem.GetDirectoryFromPath(listedPath); }

        if (listedDirectory != null) // Avoid null exception
        {
            foreach (var file in listedDirectory.Contents)
            {
                if (file.GetType() == typeof(File))
                { returnText += "F " + file.Name + "   "; }
                else
                { returnText += "D " + file.Name + "   "; }
            } 
        } 
        else { returnText = "Directory not found"; } 

        if (line.Options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}