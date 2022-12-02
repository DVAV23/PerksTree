using System;
using System.Collections;
using UnityEngine;

namespace Code.Core.Services
{
    public interface ICoroutineService
    {
        public Coroutine StartCoroutine(IEnumerator routine);
        public void StopCoroutine(Coroutine coroutine);
    }

    public class CoroutineService : BaseService, ICoroutineService
    {
        public const string SERVICE_NAME = nameof(CoroutineService);

        private MonoBehaviour _coroutinesOwner;

        public override void Init(IServicesAggregator services, Action initializedCb)
        {
            var go = new GameObject(SERVICE_NAME);
            _coroutinesOwner = go.AddComponent<CoroutineServiceComponent>();
            UnityEngine.Object.DontDestroyOnLoad(go);
            initializedCb?.Invoke();
        }

        #region ICoroutineService

        Coroutine ICoroutineService.StartCoroutine(IEnumerator routine)
        {
            return _coroutinesOwner.StartCoroutine(routine);
        }

        void ICoroutineService.StopCoroutine(Coroutine coroutine)
        {
            _coroutinesOwner.StopCoroutine(coroutine);
        }
        
        #endregion
    }
    
    public class CoroutineServiceComponent : MonoBehaviour {}
}