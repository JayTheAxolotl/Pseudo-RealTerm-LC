using System.Collections.Generic;
using LethalModDataLib.Attributes;
using LethalModDataLib.Enums;
using LethalModDataLib.Features;
using LethalModDataLib.Helpers;

namespace Pseudo_RealTerm.Classes;

public static class DirectoryHandler
{
    [ModData(SaveWhen.Manual, LoadWhen.Manual, SaveLocation.CurrentSave)]
    public static List<Directory> DirectoryList = [];
    
    public static Directory StringToDirectory(string str)
    {
        Directory returnDirectory = new Directory
        {
            IsAbsolute = str.StartsWith("/")
        };

        foreach (var part in str.Split('/'))
        {
            returnDirectory.Path.Add(part);
        }

        return returnDirectory;
    }
    
    public static bool DirectoryExists(Directory directory)
    {
        foreach (Directory d in DirectoryList)
        {
            if (directory.ToString() == d.ToString())
            {
                return true;
            }
        }
        return false;
    }
    
    public static void LoadDirs()
    {
        SaveLoadHandler.LoadData(ModDataHelper.GetModDataKey(
            typeof(DirectoryHandler), 
            nameof(DirectoryList)));
    }

    public static void SaveDirs()
    {
        SaveLoadHandler.SaveData(ModDataHelper.GetModDataKey(
            typeof(DirectoryHandler), 
            nameof(DirectoryList)));
    }
}