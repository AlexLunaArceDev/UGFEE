using System;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class LocalConfigService : IConfigService
{
    private Dictionary<string, object> _configs;

    public LocalConfigService()
    {
        FetchConfig(null);
    }

    public void CleanupService()
    {
        throw new NotImplementedException();
    }

    public void FetchConfig(Action onComplete)
    {
        FetchConfigAsync(onComplete).Forget();
    }

    private async UniTaskVoid FetchConfigAsync(Action onComplete)
    {
        TextAsset configText = (TextAsset)await Resources.LoadAsync<TextAsset>("configuration");
        if (configText == null)
        {
            Debug.LogError("Unnable to find the configuration file");
            return;
        }

        _configs = JsonConvert.DeserializeObject<Dictionary<string, object>>(configText.text);
        onComplete?.Invoke();
    }


    public T GetValue<T>(string key, T defaultValue)
    {
        if (_configs.TryGetValue(key, out object value))
        {
            if (value is JToken token)
            {
                return token.ToObject<T>();
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                Debug.LogWarning($"Could not convert value for key '{key}' to type {typeof(T)}. Returning default.");
            }
        }

        return defaultValue;
    }
}