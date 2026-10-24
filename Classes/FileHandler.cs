using System.Collections.Generic;
using HarmonyLib;
using LethalModDataLib.Attributes;
using LethalModDataLib.Enums;
using LethalModDataLib.Features;
using LethalModDataLib.Helpers;

namespace Pseudo_RealTerm.Classes;

public static class FileHandler
{
    
    [ModData(SaveWhen.Manual, LoadWhen.Manual, SaveLocation.CurrentSave)]
    public static List<File> FileList { get; set; } = [];
    
    /// Converts a full path to a name and directory string
    public static string[] SplitFullPath(string fullPath)
    {
        if (!fullPath.Contains("/")) { return [fullPath, ""]; }
        
        if (fullPath.EndsWith("/")) {fullPath.Remove(fullPath.Length - 1, 1);}
        
        string[] dirsAndFile = fullPath.Split('/');
        string finalFilename = dirsAndFile[^1];
        dirsAndFile.SetValue("", dirsAndFile.Length - 1);
        string directoryString = dirsAndFile.Join(delimiter: "/") + "/";
        
        return [finalFilename, directoryString];
    }

    public static bool FileExits(File file)
    {
        foreach (File f in FileList)
        {
            if (file.Name == f.Name && file.Directory.ToString() == f.Directory.ToString())
            {
                return true;
            }
        }
        return false;
    }
    
    public static void DebugPrintAll()
    {
        foreach (var file in FileList)
        {
            Pseudo_RealTerm.Logger.LogMessage("File at: " + file.Directory.ToString());
        }
    }

    public static void LoadFiles()
    {
        SaveLoadHandler.LoadData(ModDataHelper.GetModDataKey(
            typeof(FileHandler), 
            nameof(FileList)));
    }

    public static void SaveFiles()
    {
        SaveLoadHandler.SaveData(ModDataHelper.GetModDataKey(
            typeof(FileHandler), 
            nameof(FileList)));
    }
}