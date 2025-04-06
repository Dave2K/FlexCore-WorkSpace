using System;
using System.Collections.Generic;

namespace FlexCore.Caching.Core.Interfaces
{
    public interface ICacheFactory
    {
        ICacheProvider CreateCacheProvider(string name);
        IEnumerable<string> GetRegisteredProviders();
        void RegisterProvider(string name, Func<ICacheProvider> creator);
    }
}