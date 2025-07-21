public interface IConfigService : IService
{
    void FetchConfig(System.Action onComplete);
    T GetValue<T>(string key, T defaultValue);
}