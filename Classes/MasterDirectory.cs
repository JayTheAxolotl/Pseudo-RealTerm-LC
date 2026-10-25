using System.Collections.Generic;
using LethalModDataLib.Attributes;
using LethalModDataLib.Enums;
using Pseudo_RealTerm.Classes.FileSystem;

namespace Pseudo_RealTerm.Classes;

public static class MasterDirectory
{
    [ModData(SaveWhen.OnSave, LoadWhen.OnLoad, SaveLocation.CurrentSave)]
    public static Directory FileSystem = new Directory("/");

    
    /// Get an absolute path object that points to the given UID
    public static Path GetPathFromUID(int uid)
    {
        foreach (var file in FileSystem.Contents)
        {
            if (file.UID == uid) { return new Path("/"); }
            
            if (file.GetType() == typeof(Directory))
            {
                if (((Directory)file).ObjectIsInside(uid)) 
                { return ((Directory) file).GetPathOfObject(uid, new Path("/")); }
            }
        }
        
        return null;
    }

    /// <summary>
    /// Add a file/directory into the file system
    /// </summary>
    /// <param name="file">File/directory to be added</param>
    /// <param name="path">Path to add the file to</param>
    public static void AddFileToSystem(BaseFileType file, Path path)
    {
        if (path.PathString == "/") 
        { FileSystem.Contents.Add(file); Pseudo_RealTerm.Logger.LogDebug("Added file to root"); }
        else
        { FileSystem.GetDirectoryFromPath(path).Contents.Add(file); Pseudo_RealTerm.Logger.LogDebug("Added file to " + path.PathString); }
    }
    
}