using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static readonly Dictionary<System.Type, IService> _SERVICE_CONTAINER = new();

    //these are callbacks that get called when a service is bound to the ServiceLocator:
    private static readonly Dictionary<System.Type, List<System.Action<IService>>> _BOUND_CALLBACKS = new();
    private static readonly Dictionary<System.Type, List<System.Action>> _UNBOUND_CALLBACKS = new();

    internal class CoroutineRunner : MonoBehaviour { }
    private static CoroutineRunner _coroutineRunner;
    private static GameObject _coroutineGameObject;

    public enum BindingOption {
        /// <summary>
        /// Binds a service while ignoring new binding attempts on previously bound services.
        /// </summary>
        DEFAULT,
        
        /// <summary>
        /// Bind a service without unbinding the previous service.
        /// </summary>
        FORCE,
    }
    
    public static T Bind<T>(
        T service,
        BindingOption bindingOption = BindingOption.DEFAULT
    ) where T : class, IService
    {
        if (service == null)
        {
            throw new System.Exception(
                "Tried to bind null to ServiceLocator! Expected reference to a: " + typeof(T));
        }

        if (!_SERVICE_CONTAINER.ContainsKey(typeof(T)) || bindingOption == BindingOption.FORCE)
        {
            Debug.Log("Binding: " + typeof(T));
            _SERVICE_CONTAINER[typeof(T)] = service;

            if (_BOUND_CALLBACKS.ContainsKey(typeof(T)))
            {
                _BOUND_CALLBACKS[typeof(T)].ForEach(callback => callback?.Invoke(service));
                _BOUND_CALLBACKS.Remove(typeof(T));
            }

            return service;
        }
        
        Debug.LogWarning(typeof(T) + " already exists in ServiceLocator");
        return Get<T>();
    }

    public static bool Unbind<T>() where T : class, IService
    {
        if (!_SERVICE_CONTAINER.ContainsKey(typeof(T)))
        {
            return false;
        }

        Debug.Log("Unbinding: " + typeof(T));

        _SERVICE_CONTAINER[typeof(T)].CleanupService();
        
        if(_UNBOUND_CALLBACKS.TryGetValue(typeof(T), out var unboundCallbacks))
        {
            unboundCallbacks.ForEach(x => x?.Invoke());
            _UNBOUND_CALLBACKS.Remove(typeof(T));
        }
        
        return _SERVICE_CONTAINER.Remove(typeof(T));
    }

    public static void UnbindAll()
    {
        foreach (var kvp in _SERVICE_CONTAINER)
        {
            kvp.Value.CleanupService();
        }

        _SERVICE_CONTAINER.Clear();
    }

    public static T Get<T>() where T : class, IService
    {
        return _SERVICE_CONTAINER.ContainsKey(typeof(T)) ? (T)_SERVICE_CONTAINER[typeof(T)] : null;
    }

    public static bool TryGet<T>(out T service) where T : class, IService
    {
        service = Get<T>();
        return service != null;
    }

    public static void WhenBound<T>(System.Action<IService> callback) where T : class, IService
    {
        _SERVICE_CONTAINER.TryGetValue(typeof(T), out IService service);

        if (service != null)
        {
            callback?.Invoke((T)service);
        }
        else
        {
            if (_BOUND_CALLBACKS.ContainsKey(typeof(T)))
            {
                _BOUND_CALLBACKS[typeof(T)].Add(callback);
            }
            else
            {
                var actionList = new List<System.Action<IService>> { callback };
                _BOUND_CALLBACKS.Add(typeof(T), actionList);
            }
        }
    }

    public static void WhenUnbound<T>(System.Action callback) where T : class, IService
    {
        if (_UNBOUND_CALLBACKS.ContainsKey(typeof(T)))
        {
            _UNBOUND_CALLBACKS[typeof(T)].Add(callback);
        }
        else
        {
            var actionList = new List<System.Action> { callback };
            _UNBOUND_CALLBACKS.Add(typeof(T), actionList);
        }
    }

    public static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        if (_coroutineGameObject == null)
        {
            _coroutineGameObject = new GameObject("ServiceLocator.CoroutineRunner");
            _coroutineRunner = _coroutineGameObject.AddComponent<CoroutineRunner>();
#if UNITY_EDITOR
            if (!Application.isPlaying) return null;
#endif
            Object.DontDestroyOnLoad(_coroutineGameObject);
        }
        
        return _coroutineRunner.StartCoroutine(coroutine);
    }

    public static void StopCoroutine(ref Coroutine coroutine)
    {
        if (_coroutineGameObject == null)
        {
            _coroutineGameObject = new GameObject("ServiceLocator.CoroutineRunner");
            _coroutineRunner = _coroutineGameObject.AddComponent<CoroutineRunner>();
        }

        if (coroutine == null) return;
        _coroutineRunner.StopCoroutine(coroutine);
        coroutine = null;
    }
}