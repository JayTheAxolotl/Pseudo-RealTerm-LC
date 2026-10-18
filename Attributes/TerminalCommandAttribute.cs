using System;

namespace Pseudo_RealTerm.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TerminalCommandAttribute : Attribute
{
    public TerminalCommandAttribute() {}
}