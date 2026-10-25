namespace Pseudo_RealTerm.Classes.FileSystem;

public abstract class BaseFileType
{
    public string Name;
    
    /// Unique Identifier
    public int UID;
    
    /// <summary>
    /// Get the path of this object
    /// </summary>
    /// <param name="includeSelf">If its self is included in the path at the end</param>
    /// <returns></returns>
    public abstract Path GetPath(bool includeSelf);
}