using Gramium.Framework.Commands.Interfaces;
using Gramium.Framework.Commands.Models;
using Gramium.Framework.Context;

namespace Gramium.Framework.Commands;

public abstract class CommandBase : ICommandHandler, ICommandParameters, ICommandValidation
{
    public abstract string Command { get; }
    public virtual string Description => string.Empty;
    public virtual string[] Aliases => [];
    public virtual CommandParameter[] Parameters => [];

    public virtual ValidationResult Validate(string[] args)
    {
        if (Parameters.Length == 0) return ValidationResult.Success();

        var requiredParams = Parameters.Count(p => !p.IsOptional);
        if (args.Length < requiredParams)
        {
            return ValidationResult.Error(
                $"Недостаточно параметров. Требуется минимум {requiredParams}, получено {args.Length}");
        }

        for (var i = 0; i < args.Length && i < Parameters.Length; i++)
        {
            var param = Parameters[i];
            var arg = args[i];

            if (!TryParseParameter(arg, param.Type, out _))
            {
                return ValidationResult.Error(
                    $"Неверный формат параметра '{param.Name}'. Ожидается {param.Type.Name}");
            }
        }

        return ValidationResult.Success();
    }

    protected object?[] ParseParameters(string[] args)
    {
        var result = new object?[Parameters.Length];
    
        for (var i = 0; i < Parameters.Length; i++)
        {
            var param = Parameters[i];
            var value = i < args.Length ? args[i] : param.DefaultValue;
    
            if (value == null && !param.IsOptional)
            {
                throw new ArgumentException($"Отсутствует обязательный параметр '{param.Name}'");
            }
    
            if (value != null)
            {
                if (!TryParseParameter(value.ToString()!, param.Type, out var parsedValue))
                {
                    throw new ArgumentException($"Неверный формат параметра '{param.Name}'");
                }
    
                result[i] = parsedValue;
            }
            else
            {
                result[i] = null;
            }
        }
    
        return result;
    }

    private static bool TryParseParameter(string value, Type type, out object? result)
    {
        try
        {
            if (type == typeof(string))
            {
                result = value;
                return true;
            }

            if (type == typeof(int) && int.TryParse(value, out var intResult))
            {
                result = intResult;
                return true;
            }

            if (type == typeof(long) && long.TryParse(value, out var longResult))
            {
                result = longResult;
                return true;
            }

            if (type == typeof(decimal) && decimal.TryParse(value, out var decimalResult))
            {
                result = decimalResult;
                return true;
            }

            result = null;
            return false;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public abstract Task HandleAsync(IMessageContext context, CancellationToken ct = default);
}