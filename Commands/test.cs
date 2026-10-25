using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Classes.FileSystem;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands;

[TerminalCommand]
public static class testcomd
{
    public static string Command = "di";

    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "di [options] [arguments]\n" +
                             "Options:\n" +
                             "--help : display this text\n" +
                             "-c     : create a file or directory\n" +
                             "-d     : specify directory for -c\n" +
                             "-f     : specify file for -c\n" +
                             "-l     : list things in a directory";
        const bool replaceText = false;

        string returnText = "";

        if (line.Options.Contains("-c"))
        {
            if (line.Options.Contains("-d"))
            {
                Directory newDirectory = new Directory(line.Arguments[0]);
                Path directoryPath = new Path(line.Arguments[1]);
                MasterDirectory.AddFileToSystem(newDirectory, directoryPath);
            } 
            else if (line.Options.Contains("-f"))
            {
                File newDirectory = new File(line.Arguments[0]);
                Path directoryPath = new Path(line.Arguments[1]);
                MasterDirectory.AddFileToSystem(newDirectory, directoryPath);
            }
        }
        else if (line.Options.Contains("-l"))
        {
            if (line.Arguments[0] != "/")
            {
                foreach (var file in MasterDirectory.FileSystem.GetDirectoryFromPath(new Path(line.Arguments[0])).Contents)
                {
                    returnText += file.Name + "\n";
                    Pseudo_RealTerm.Logger.LogMessage(file.Name);
                }
            }
            else
            {
                foreach (var file in MasterDirectory.FileSystem.Contents)
                {
                    returnText += file.Name + "\n";
                    Pseudo_RealTerm.Logger.LogMessage(file.Name);
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