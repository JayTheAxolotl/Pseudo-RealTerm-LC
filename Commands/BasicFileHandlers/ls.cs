using Pseudo_RealTerm.Attributes;
using Pseudo_RealTerm.Classes;
using Pseudo_RealTerm.Patches;

namespace Pseudo_RealTerm.Commands.BasicFileHandlers;

[TerminalCommand]
public static class ls
{
    public static string Command = "ls";
    
    public static void Run(Terminal instance, TermLine line)
    {
        const string usage = "ls [options] [dir]\n"+ 
                             "Options:\n" + 
                             "--help : display this text";
        const bool replaceText = false;
        
        string returnText = "";     
        
        FileHandler.LoadFiles();
        foreach (var file in FileHandler.FileList)
        {
            returnText += file.Name + "   ";
        }

        if (line.options.Contains("--help"))
        {
            returnText = usage;
        }
        
        TerminalPatch.WriteToTerminal(returnText, instance, replaceText);
    }
}