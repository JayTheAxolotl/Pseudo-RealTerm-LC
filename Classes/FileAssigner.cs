using Pseudo_RealTerm.Enums;

namespace Pseudo_RealTerm.Classes;

public static class FileAssigner
{
    public static FileTypes AssignFromExtension(string extension)
    {
        return extension switch
        {
            "txt" => FileTypes.Text,
            "moon" => FileTypes.Moon,
            _ => FileTypes.Unknown
        };
    }
}