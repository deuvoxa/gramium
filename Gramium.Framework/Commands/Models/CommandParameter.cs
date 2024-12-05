namespace Gramium.Framework.Commands.Models;

public class CommandParameter(
    string name,
    Type type,
    bool isOptional = false,
    object? defaultValue = null,
    string description = "")
{
    public string Name { get; } = name;
    public Type Type { get; } = type;
    public bool IsOptional { get; } = isOptional;
    public object? DefaultValue { get; } = defaultValue;
    public string Description { get; } = description;
}