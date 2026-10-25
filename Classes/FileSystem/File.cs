namespace Pseudo_RealTerm.Classes.FileSystem;

public class File : BaseFileType
{
    public File(string name)
    {
        UID = Pseudo_RealTerm.NextUID();
        Name = name;
    }

    public override Path GetPath(bool includeSelf = false)
    {
        if (includeSelf) { return MasterDirectory.GetPathFromUID(UID).Append(Name); }
        return MasterDirectory.GetPathFromUID(UID);
    }
}