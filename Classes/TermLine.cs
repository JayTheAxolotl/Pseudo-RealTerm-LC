using System.Collections.Generic;
using System.Linq;

namespace Pseudo_RealTerm.Classes;

/// Class for parsing lines from the terminal, with a command, Arguments, and Options
public class TermLine 
{
    private string[] _rawLine;
    
    /// The first word in the given line
    public string Command; 
    
    /// Words in the line that start with a hyphen ("-") or double hyphen ("--")
    public List<string> Options = []; // Set so it is never null
    
    /// Every other word in the line that was not the Command, or an Option
    public List<string> Arguments = []; // Set so it is never null
    
    
    public TermLine(string rawLine)
    {
        this._rawLine = rawLine.ToLower().Split(' ');
        this.Command = this._rawLine[0];

        foreach (var thing in _rawLine)
        {
            if (thing == this.Command) { continue; } // Skip command
            
            // If it is an option, add it to Options
            if (thing.StartsWith("--")) // Full word options
            { this.Options.Add(thing); }
            else if (thing.StartsWith("-")) // Single letter options
            {
                // Done this way to ensure combined single letter options are equivalent to not combined ones
                // E.G "ls -al" == "ls -a -l"
                foreach (var option in thing)
                {
                    if (option == '-') { continue; }
                    this.Options.Add("-" + option);
                }
            }
            else // If it isn't an option then it's an argument
            { this.Arguments.Add(thing); }
        }
    }
    
    public void debugPrint()
    {
        foreach (var thing in this._rawLine)
        {
            Pseudo_RealTerm.Logger.LogMessage("thing: " + thing);
        }
        
        Pseudo_RealTerm.Logger.LogMessage("Command " + this.Command);
        
        foreach (var option in this.Options)
        {
            Pseudo_RealTerm.Logger.LogMessage("Option: " + option);
        }
        
        foreach (var argument in this.Arguments)
        {
            Pseudo_RealTerm.Logger.LogMessage("Arg: " + argument);
        }
    }

}