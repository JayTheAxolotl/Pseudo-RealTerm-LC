using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace Pseudo_RealTerm.Classes.FileSystem;

public class Path
{
    public string PathString;
    public List<string> PathList;
    public bool IsAbsolute;

    public Path(string path)
    {
        PathString = path;
        
        IsAbsolute = PathString.StartsWith("/");

        if (IsAbsolute)
        {
            PathList = PathString.Split("/").ToList();
            PathList.RemoveAt(0);
        }
        else
        { PathList = PathString.Split('/').ToList(); }
    }
    
    /// Returns a new path with the given path added onto it
    public Path Append(string path)
    {
        path = path.Replace(" ", "");
        if (!path.StartsWith("/") && PathString != "/") 
        { return new Path(PathString + "/" + path);}
        else { return new Path(PathString + path); }
    }

    /// Returns a new path with the last part of this path removed
    public Path Pop()
    {
        List<string> poppedList;
        if (PathList.Count > 0)
        { poppedList = PathList.GetRange(0, PathList.Count - 1); }
        else { poppedList = PathList; }

        if (poppedList.Count == 0 && IsAbsolute) 
        { return new Path("/"); }
        else if (poppedList.Count == 0) 
        { return new Path(""); }
        else if (IsAbsolute)
        { return new Path("/" + poppedList.Join(delimiter: "/")); }
        else
        { return new Path(poppedList.Join(delimiter: "/")); }
    }

    public static bool WouldBeAbsolute(string pathString)
    {
        return pathString.StartsWith("/");
    }

    public override string ToString() { return PathString; }
}