using System.Collections.Generic;
using System.Linq;

namespace Pseudo_RealTerm.Classes;

public class TermLine // Class for parsing lines in the terminal
{

    public string Command;
    public string[] RawLine;
    
    public List<string> arguments = new List<string>(); // Set so they are never null
    public List<string> options = new List<string>();
    
    public TermLine(string rawLine)
    {
        this.RawLine = rawLine.ToLower().Split(' ');
        this.Command = this.RawLine[0];
        
        // i starts at 1 to skip this.Command
        for (int i = 1; i < this.RawLine.Length; i++)
        {
            // If it is an option, add it to options
            if (this.RawLine[i].StartsWith("-")) 
            { this.options.Add(this.RawLine[i]); }
            else // If it isn't then it's an argument
            { this.arguments.Add(this.RawLine[i]); }
        }
        
    }
    
    public void debugPrint()
    {
        foreach (var thing in this.RawLine)
        {
            Pseudo_RealTerm.Logger.LogMessage("thing: " + thing);
        }
        
        Pseudo_RealTerm.Logger.LogMessage("Command " + this.Command);
        
        foreach (var option in this.options)
        {
            Pseudo_RealTerm.Logger.LogMessage("option: " + option);
        }
        
        foreach (var argument in this.arguments)
        {
            Pseudo_RealTerm.Logger.LogMessage("arg: " + argument);
        }
    }

}