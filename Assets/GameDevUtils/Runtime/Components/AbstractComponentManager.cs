using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameDevUtils.Runtime
{
    public interface IManagedComponent
    {
        
    }
    
    public abstract class AbstractComponentManager<T, TS> : UnitySingleton<TS> 
        where T : IManagedComponent 
        where TS : MonoBehaviour
    {
        [field: SerializeField] public List<T> AllComponents { get; private set; } = new List<T>();
        
        public virtual void Register(T element)
        {
            AllComponents.Add(element);
            
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            
            _onRegister?.Invoke();
            _onRegisterComponent?.Invoke(element);
        }

        public virtual void Unregister(T element)
        {
            AllComponents.Remove(element);
            
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif
            
            _onUnregister?.Invoke();
            _onUnregisterComponent?.Invoke(element);
        }
        
        void OnDestroy()
        {
            _onRegister.RemoveAllListeners();
            _onRegisterComponent.RemoveAllListeners();
            _onUnregister.RemoveAllListeners();
            _onUnregisterComponent.RemoveAllListeners();
        }

        #region Events

        readonly TemplateEvent _onRegister = new TemplateEvent();
        readonly TemplateEvent<T> _onRegisterComponent = new TemplateEvent<T>();

        readonly TemplateEvent _onUnregister = new TemplateEvent();
        readonly TemplateEvent<T> _onUnregisterComponent = new TemplateEvent<T>();
        
        public void AddListenerOnRegister(UnityAction action) => _onRegister.AddListener(action);
        public void RemoveListenerOnRegister(UnityAction action) => _onRegister.RemoveListener(action);
        public void AddListenerOnRegisterComponent(UnityAction<T> action) => _onRegisterComponent.AddListener(action);
        public void RemoveListenerOnRegisterComponent(UnityAction<T> action) => _onRegisterComponent.RemoveListener(action);
        public void AddListenerOnUnregister(UnityAction action) => _onUnregister.AddListener(action);
        public void RemoveListenerOnUnregister(UnityAction action) => _onUnregister.RemoveListener(action);
        public void AddListenerOnUnregisterComponent(UnityAction<T> action) => _onUnregisterComponent.AddListener(action);
        public void RemoveListenerOnUnregisterComponent(UnityAction<T> action) => _onUnregisterComponent.RemoveListener(action);

        #endregion
    }
}