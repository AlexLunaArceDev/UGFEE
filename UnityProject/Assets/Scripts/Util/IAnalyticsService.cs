using System.Collections.Generic;

public interface IAnalyticsService : IService
{
    void TrackEvent(string eventName, Dictionary<string, object> parameters = null);
}
