namespace Pseudo_RealTerm.Classes;

public class File
{
    public string Name = "";
    public string Type = "";
    public string Contents = "";
    public Directory Directory = new Directory();

    public bool IsFoundUnder(Directory directory)
    {
        if (Directory.IsSubdirOf(directory)){ return true; }
        return false;
    }
}