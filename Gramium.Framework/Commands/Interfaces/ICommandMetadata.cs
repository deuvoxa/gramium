using Gramium.Framework.Commands.Models;

namespace Gramium.Framework.Commands.Interfaces;

public interface ICommandMetadata
{
    string Command { get; }
    string Description { get; }
    string[] Aliases { get; }
}

public interface ICommandParameters
{
    CommandParameter[] Parameters { get; }
}

public interface ICommandValidation
{
    ValidationResult Validate(string[] args);
}