using System;

namespace Virtek.Base.ServiceLocationFoundation
{
    public interface IServiceLocationFoundation : IDisposable
    {
        void Start();
        void Stop();
    }
}
