namespace FlexCore.Core.Configuration.Models
{
    /// <summary>
    /// Rappresenta le impostazioni di configurazione per i diversi provider ORM.
    /// </summary>
    public class ORMSettings
    {
        /// <summary>
        /// Ottiene o imposta il provider ORM predefinito (ad esempio, EFCore, Dapper, ADO).
        /// </summary>
        public required string DefaultProvider { get; set; }

        /// <summary>
        /// Ottiene o imposta la lista dei provider ORM supportati (ad esempio, EFCore, Dapper, ADO).
        /// </summary>
        public List<string> Providers { get; set; } = new List<string>();

        /// <summary>
        /// Ottiene o imposta le impostazioni di configurazione per EF Core.
        /// </summary>
        public required EFCoreSettings EFCore { get; set; }

        /// <summary>
        /// Ottiene o imposta le impostazioni di configurazione per Dapper.
        /// </summary>
        public required DapperSettings Dapper { get; set; }

        /// <summary>
        /// Ottiene o imposta le impostazioni di configurazione per ADO.NET.
        /// </summary>
        public required ADOSettings ADO { get; set; }
    }

    /// <summary>
    /// Rappresenta le impostazioni di configurazione specifiche per EF Core.
    /// </summary>
    public class EFCoreSettings
    {
        /// <summary>
        /// Ottiene o imposta un valore che indica se il lazy loading delle entità correlate è abilitato in EF Core.
        /// </summary>
        public required bool EnableLazyLoading { get; set; }

        /// <summary>
        /// Ottiene o imposta un valore che indica se il logging dei dati sensibili è abilitato in EF Core.
        /// </summary>
        public required bool EnableSensitiveDataLogging { get; set; }
    }

    /// <summary>
    /// Rappresenta le impostazioni di configurazione specifiche per Dapper.
    /// </summary>
    public class DapperSettings
    {
        /// <summary>
        /// Ottiene o imposta il timeout dei comandi (in secondi) per le query Dapper.
        /// </summary>
        public required int CommandTimeout { get; set; }
    }

    /// <summary>
    /// Rappresenta le impostazioni di configurazione specifiche per ADO.NET.
    /// </summary>
    public class ADOSettings
    {
        /// <summary>
        /// Ottiene o imposta il timeout di connessione (in secondi) per le connessioni ADO.NET.
        /// </summary>
        public required int ConnectionTimeout { get; set; }
    }
}
