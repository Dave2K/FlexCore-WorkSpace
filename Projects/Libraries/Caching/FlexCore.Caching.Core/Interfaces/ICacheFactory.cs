namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>
    /// Definisce un factory per creare e registrare provider di cache.
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        /// Crea un provider di cache in base al nome registrato.
        /// </summary>
        /// <param name="name">Nome del provider.</param>
        ICacheProvider CreateCacheProvider(string name);

        /// <summary>
        /// Registra un provider di cache.
        /// </summary>
        /// <param name="name">Nome del provider.</param>
        /// <param name="creator">Funzione di creazione del provider.</param>
        void RegisterProvider(string name, Func<ICacheProvider> creator);
    }
}