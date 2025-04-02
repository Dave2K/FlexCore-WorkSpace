namespace FlexCore.Core.Exceptions;

/// <summary>
/// Classe base astratta per tutte le eccezioni personalizzate dell'applicazione.
/// </summary>
public abstract class AppException : Exception
{
    /// <summary>
    /// Codice univoco identificativo dell'errore (formato: "PREFISSO-NUMERO").
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Categoria funzionale dell'errore (es. "Database", "Caching").
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Inizializza una nuova istanza della classe <see cref="AppException"/>.
    /// </summary>
    /// <param name="errorCode">Codice identificativo dell'errore.</param>
    /// <param name="category">Categoria funzionale dell'errore.</param>
    /// <param name="message">Messaggio descrittivo dell'errore.</param>
    protected AppException(string errorCode, string category, string message)
        : base(message)
    {
        ErrorCode = errorCode;
        Category = category;
    }
}