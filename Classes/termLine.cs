using System.Collections.Generic;
using System.Linq;

namespace Pseudo_RealTerm.Classes;

public class termLine // Class for parsing lines in the terminal
{

    public string command;
    public string[] rawLine;
    
    public List<string> arguments = new List<string>(); // Set so they are never null
    public List<string> options = new List<string>();
    
    public termLine(string rawLine)
    {
        this.rawLine = rawLine.ToLower().Split(' ');
        this.command = this.rawLine[0];
        
        // i starts at 1 to skip this.command
        for (int i = 1; i < this.rawLine.Length; i++)
        {
            // If it is an option, add it to options
            if (this.rawLine[i].StartsWith("-")) 
            { this.options.Add(this.rawLine[i]); }
            else // If it isn't then it's an argument
            { this.arguments.Add(this.rawLine[i]); }
        }
        
    }
    
    public void debugPrint()
    {
        foreach (var thing in this.rawLine)
        {
            Pseudo_RealTerm.Logger.LogMessage("thing: " + thing);
        }
        
        Pseudo_RealTerm.Logger.LogMessage("command " + this.command);
        
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