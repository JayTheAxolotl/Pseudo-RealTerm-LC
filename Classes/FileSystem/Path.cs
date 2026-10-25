using System;
using System.Collections.Generic;
using System.Linq;

namespace Pseudo_RealTerm.Classes.FileSystem;

public class Path
{
    public string PathString;
    public string[] PathList;
    public bool IsAbsolute;

    public Path(string path)
    {
        PathString = path;
        
        IsAbsolute = PathString.StartsWith("/");

        if (IsAbsolute)
        {
            List<string> PathListL = PathString.Split("/").ToList();
            PathListL.RemoveAt(0);
            PathList = PathListL.ToArray();
        }
        else
        { PathList = PathString.Split('/'); }
    }
    
    /// Returns a new path with the given path added onto it
    public Path Append(string path)
    {
        path = path.Replace(" ", "");
        if (!path.StartsWith("/") && PathString != "/") 
        { return new Path(PathString + "/" + path);}
        else { return new Path(PathString + path); }
    }

    public static bool WouldBeAbsolute(string pathString)
    {
        return pathString.StartsWith("/");
    }

    public override string ToString() { return PathString; }
}