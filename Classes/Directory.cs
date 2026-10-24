using System.Collections.Generic;
using HarmonyLib;

namespace Pseudo_RealTerm.Classes;

public class Directory
{
    public List<string> Path = [];
    public bool IsAbsolute;

    public override string ToString()
    {
        if (IsAbsolute && (Path.Count == 0 || Path[0] == "")) { return "/"; } 
        if (IsAbsolute) {return "/" + Path.Join(delimiter: "/") + "/";}
        return Path.Join(delimiter: "/") + "/";
    }

    public void SetFromStringDirectory(string str)
    {
        IsAbsolute = str.StartsWith("/");
        Path = [];

        foreach (var part in str.Split('/'))
        {
            Path.Add(part);
        }
    }

    public bool IsSubdirOf(Directory possibleParent)
    {
        if (IsAbsolute && possibleParent.IsAbsolute)
        {
            if (ToString().StartsWith(possibleParent.ToString())) { return true; }
        }
        return false;
    }
}