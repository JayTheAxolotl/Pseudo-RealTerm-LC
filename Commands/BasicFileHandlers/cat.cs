using System;
using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.BasicFileHandlers;

[TerminalCommand]
public static class cat
{
    public static string Command = "cat";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "cat [options] [arguments]\n" +
                             "Options:\n" +
                             "--help : display this text";
        const bool replaceText = false;

        string returnText = "";

        if (line.arguments.Count == 0)
        {
            returnText = "No arguments given\n" + usage;
        }
        else
        {
            string[] splitFilePath;
            if (line.arguments[0].StartsWith("/"))
            {
                splitFilePath = FileHandler.SplitFullPath(line.arguments[0]);
            }
            else
            {
                splitFilePath = FileHandler.SplitFullPath(line.LineDirectory.ToString() + line.arguments[0]);
            }

            FileHandler.LoadFiles();
            foreach (var file in FileHandler.FileList)
            {
                if (file.Name == splitFilePath[0] && file.Directory.ToString() == splitFilePath[1])
                {
                    returnText = file.Contents + "\n";
                    break;
                }
            }
            
            if (returnText == "")
            {
                returnText = "File not found";
            }
        }

        if (line.options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}