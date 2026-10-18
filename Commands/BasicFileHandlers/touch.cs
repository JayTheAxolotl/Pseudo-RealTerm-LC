using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.BasicFileHandlers;

[TerminalCommand]
public static class touch
{
    public static string Command = "touch";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "touch [options] [filename]\n"+ 
                "Options:\n" + 
                "--help : display this text";
        const bool replaceText = false;
        
        string returnText;

        FileHandler.LoadFiles();

        if (line.arguments.Count > 0)
        {
            string[] dirAndName = FileHandler.SplitFullPath(line.arguments[0]);
            if (!dirAndName[1].StartsWith("/")) {dirAndName[1] = line.LineDirectory + dirAndName[1];} // Relative path if absolute is not given
                        
            returnText = "made file: \"" + dirAndName[0] + "\" at \"" + dirAndName[1] + "\"" ;
                        
            File newFile = new File
            {
                Name = dirAndName[0],
                Directory = DirectoryHandler.StringToDirectory(dirAndName[1])
            };

            if (FileHandler.FileExits(newFile))
            {
                returnText = "File already exists";
            }
            else
            {
                FileHandler.FileList.Add(newFile);
                FileHandler.SaveFiles();
            }
        }
        else
        { returnText = "No arguments given\n" + usage; }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}