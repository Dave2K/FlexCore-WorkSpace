namespace FlexCore.Database.Interfaces;

/// <summary>
/// Interfaccia generica per le query nel pattern CQRS.
/// </summary>
/// <typeparam name="TResult">Tipo del risultato restituito.</typeparam>
public interface IQuery<TResult>
{
    /// <summary>
    /// Esegue la query e restituisce il risultato.
    /// </summary>
    /// <returns>Risultato della query.</returns>
    TResult Execute();
}