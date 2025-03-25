namespace FlexCore.Infrastructure.Commands;

/// <summary>
/// Implementazione base di un comando.
/// </summary>
public abstract class CommandBase : ICommand
{
    public abstract void Execute();
}
