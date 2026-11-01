using Pseudo_RealTerm.Enums;

namespace Pseudo_RealTerm.Classes.FileSystem;

public class File : BaseFileType
{

    public string Contents = "";
    public FileTypes Extension = FileTypes.Unknown;
    
    public File(string name)
    {
        UID = Pseudo_RealTerm.NextUID();
        Name = name;

        if (Name.Contains("."))
        { Extension = FileAssigner.AssignFromExtension(Name.Split('.')[Name.Split('.').Length - 1]); }
    }

    public override Path GetPath(bool includeSelf = false)
    {
        if (includeSelf) { return MasterDirectory.GetPathFromUID(UID).Append(Name); }
        return MasterDirectory.GetPathFromUID(UID);
    }
}