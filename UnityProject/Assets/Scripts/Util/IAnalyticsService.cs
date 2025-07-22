using System.Collections.Generic;

public interface IAnalyticsService : IService
{
    void Register(Dictionary<string, object> parameters = null);
    void Track(string eventName, Dictionary<string, object> parameters = null);
    void Screen(UIScreenBase screen, Dictionary<string, object> parameters = null);
    void Flush();
}
