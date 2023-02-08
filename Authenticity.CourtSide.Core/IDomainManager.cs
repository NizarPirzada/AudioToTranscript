using System;

namespace Authenticity.CourtSide.Core
{
    public interface IDomainManager : IDisposable
    {
        T CreateProxy<T>() where T : class, IDomain;
    }
}