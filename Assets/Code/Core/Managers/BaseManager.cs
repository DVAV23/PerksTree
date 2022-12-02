using System;
using Code.Core.Services;

namespace Code.Core.Managers
{
    public abstract class BaseManager
    {
        public abstract void Init(IServicesAggregator services, Action initializedCb);
    }
}