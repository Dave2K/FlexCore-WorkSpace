namespace FlexCore.Caching.Core.Interfaces
{
    /// <summary>
    /// Definisce un meccanismo factory per la creazione e registrazione di provider di cache
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        /// Crea un'istanza del provider di cache registrato
        /// </summary>
        /// <param name="name">Nome univoco del provider (case-insensitive)</param>
        /// <returns>Istanza del provider di cache</returns>
        /// <exception cref="ArgumentException">Se il nome non è registrato</exception>
        ICacheProvider CreateCacheProvider(string name);

        /// <summary>
        /// Registra un provider di cache nella factory
        /// </summary>
        /// <param name="name">Nome univoco del provider (case-insensitive)</param>
        /// <param name="creator">Factory method per creare il provider</param>
        /// <exception cref="ArgumentNullException">Se name o creator sono null</exception>
        void RegisterProvider(string name, Func<ICacheProvider> creator);
    }
}