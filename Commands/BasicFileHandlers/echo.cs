using System;
using HarmonyLib;
using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.BasicFileHandlers;

[TerminalCommand]
public static class echo
{
    public static string Command = "echo";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "echo [options] [arguments]\n" +
                "Options:\n" +
                "--help : display this text";
        const bool replaceText = false;
        
        string returnText;

        if (line.arguments.Count > 0)
        {
            returnText = line.arguments.Join(delimiter: " ") + "\n";

            if (line.arguments.Contains(">"))
            {
                var text = new ArraySegment<string>(line.arguments.ToArray(), 0, line.arguments.IndexOf(">") + 1);
                var filePath = line.arguments[line.arguments.IndexOf(">") + 1];
                
                string[] splitFilePath;
                if (filePath.StartsWith("/"))
                {
                    splitFilePath = FileHandler.SplitFullPath(filePath);
                }
                else
                {
                    splitFilePath = FileHandler.SplitFullPath(line.LineDirectory.ToString() + filePath);
                }
                
                FileHandler.LoadFiles();
                foreach (var file in FileHandler.FileList)
                {
                    if (file.Name == splitFilePath[0] && file.Directory.ToString() == splitFilePath[1])
                    {
                        file.Contents = text.Join(delimiter: " ");
                        FileHandler.SaveFiles();
                        break;
                    }
                }
            }
            
            if (line.arguments.Contains(">>"))
            {
                var text = new ArraySegment<string>(line.arguments.ToArray(), 0, line.arguments.IndexOf(">>") + 1);
                var filePath = line.arguments[line.arguments.IndexOf(">>") + 1];
                
                string[] splitFilePath;
                if (filePath.StartsWith("/"))
                {
                    splitFilePath = FileHandler.SplitFullPath(filePath);
                }
                else
                {
                    splitFilePath = FileHandler.SplitFullPath(line.LineDirectory.ToString() + filePath);
                }
                
                FileHandler.LoadFiles();
                foreach (var file in FileHandler.FileList)
                {
                    if (file.Name == splitFilePath[0] && file.Directory.ToString() == splitFilePath[1])
                    {
                        file.Contents += text.Join(delimiter: " ");
                        FileHandler.SaveFiles();
                        break;
                    }
                }
            }
                
        }
        else
        { returnText = "No arguments given\n" + usage; }

        if (line.options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}