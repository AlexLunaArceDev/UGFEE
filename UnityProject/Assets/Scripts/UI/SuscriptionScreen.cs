using System.Collections.Generic;
using UnityEngine;

public class SuscriptionScreen : UIScreenBase
{
    protected Dictionary<string, object> _screenConfiguration;

    protected IAnalyticsService analyticsService = ServiceLocator.Get<IAnalyticsService>();

    [SerializeField]
    protected GameObject _nextUI;
    
    virtual public void InitializeScreen(Dictionary<string, object> newScreenConfig)
    {
        _screenConfiguration = newScreenConfig;
        analyticsService.Screen(this, null);
    }

    public override void Next()
    {
        analyticsService.Track("startedFreeTrail", new Dictionary<string, object>
        {
            { "currentScreen", ScreenName}
        });

        analyticsService.Flush();

        if (_nextUI)
        {
            Instantiate(_nextUI, transform.parent);
            DestroyImmediate(gameObject);
        }
    }
}
