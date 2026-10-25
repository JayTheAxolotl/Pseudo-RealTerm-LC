using System;
using System.Collections.Generic;

namespace Pseudo_RealTerm.Classes.FileSystem;

public class Directory : BaseFileType
{

    public List<BaseFileType> Contents = [];
    
    public Directory(string name)
    {
        UID = Pseudo_RealTerm.NextUID();
        Name = name;
    }
    
    public override Path GetPath(bool includeSelf = true)
    {
        if (includeSelf) { return MasterDirectory.GetPathFromUID(UID).Append(Name); }
        return MasterDirectory.GetPathFromUID(UID);
    }

    public bool ObjectIsInside(int uid)
    {
        foreach (var file in Contents)
        {
            if (file.UID == uid) { return true; }

            if (file.GetType() == typeof(Directory))
            {
                return ((Directory) file).ObjectIsInside(uid);
            }
        }

        return false;
    }

    public Path GetPathOfObject(int uid, Path currentPath)
    {
        foreach (var file in Contents)
        {
            if (file.UID == uid) {return currentPath.Append(Name);}

            if (file.GetType() == typeof(Directory))
            {
                return ((Directory) file).GetPathOfObject(uid, currentPath.Append(Name));
            }
        }

        return null;
    }

    public Directory GetDirectoryFromPath(Path path, int depth = 0)
    {
        foreach (var directory in Contents)
        {
            if (directory.GetType() == typeof(Directory))
            {
                if (directory.Name == path.PathList[depth] 
                    && depth == path.PathList.Length - 1) // Ensure the search is at the end of the path, and not in the middle before returning
                {return (Directory) directory;}
                
                return ((Directory) directory).GetDirectoryFromPath(path, depth + 1);
            }
        }
        
        return null;
    }
}