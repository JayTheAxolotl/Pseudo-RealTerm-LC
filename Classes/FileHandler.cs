using System.Collections.Generic;
using HarmonyLib;
using LethalModDataLib.Attributes;
using LethalModDataLib.Enums;
using LethalModDataLib.Features;
using LethalModDataLib.Helpers;

namespace Pseudo_RealTerm.Classes;

public struct File
{
    public string Name;
    public string Path;
    public string Type;
    public bool IsDirectory;
    public string Contents;
}

public static class FileHandler
{
    
    [ModData(SaveWhen.OnSave, LoadWhen.Manual, SaveLocation.CurrentSave)]
    public static List<File> FileList { get; set; } = [];
    public static string[] FileNameToPath(string filename)
    {
        if (!filename.Contains("/")) { return [filename, ""]; }
        
        if (filename.EndsWith("/")) {filename.Remove(filename.Length - 1, 1);}
        
        string[] dirsAndFile = filename.Split('/');
        string finalFilename = dirsAndFile[dirsAndFile.Length - 1];
        dirsAndFile.SetValue("", dirsAndFile.Length - 1);
        string dirs = dirsAndFile.Join(delimiter: "/");
        
        return [finalFilename, dirs];
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