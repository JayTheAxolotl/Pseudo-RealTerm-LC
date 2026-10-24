using System.Collections.Generic;
using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands;

[TerminalCommand]
public static class rm
{
    public static string Command = "rm";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "rm [options] [arguments]\n" +
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
            Directory deletedDir;
            if (line.arguments[0].StartsWith("/"))
            {
                splitFilePath = FileHandler.SplitFullPath(line.arguments[0]);
                deletedDir = DirectoryHandler.StringToDirectory(line.arguments[0]);
            }
            else
            {
                splitFilePath = FileHandler.SplitFullPath(line.LineDirectory.ToString() + line.arguments[0]);
                deletedDir = DirectoryHandler.StringToDirectory(line.LineDirectory.ToString() + line.arguments[0]);
            }
            
            DirectoryHandler.LoadDirs();            
            FileHandler.LoadFiles();

            if (line.options.Contains("-r"))
            {
                if (DirectoryHandler.DirectoryExists(deletedDir))
                {
                    returnText = deletedDir.ToString();
                    DirectoryHandler.DirectoryList.Remove(deletedDir);

                    List<Directory> dirsToRemove = [];
                    foreach (var directory in DirectoryHandler.DirectoryList)
                    {
                        if (directory.IsSubdirOf(deletedDir))
                        {
                            dirsToRemove.Add(directory);
                        }
                    }

                    foreach (var directory in dirsToRemove)
                    {
                        DirectoryHandler.DirectoryList.Remove(directory);
                    }

                    foreach (var file in FileHandler.FileList)
                    {
                        if (file.IsFoundUnder(deletedDir))
                        {
                            FileHandler.FileList.Remove(file);
                        }
                    }
                }
                else
                {
                    returnText = "Directory not found";
                }
            }
            
            foreach (var file in FileHandler.FileList)
            {
                if (file.Name == splitFilePath[0] && file.Directory.ToString() == splitFilePath[1])
                {
                    returnText = file.Name;
                    FileHandler.FileList.Remove(file);
                    break;
                }
            }

            if (returnText == "")
            {
                returnText = "File not found, use -r for directories";
            }
            
            DirectoryHandler.SaveDirs();
            FileHandler.SaveFiles();
            
            DirectoryHandler.DebugPrintAll();
            FileHandler.DebugPrintAll();
        }

        if (line.options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}