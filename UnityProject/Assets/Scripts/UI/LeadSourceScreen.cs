using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeadSourceScreen : UIScreenBase
{
    private ToggleGroup _toggleGroup;
    private Toggle _activeToggle;
    private int _timesToggleChanged = 0;

    [SerializeField] GameObject _freeTrailUI;
    [SerializeField] GameObject _payWallUI;
    IAnalyticsService analyticsService = ServiceLocator.Get<IAnalyticsService>();

    private void Awake()
    {
        _toggleGroup = GetComponentInChildren<ToggleGroup>();
        analyticsService.Screen(this, null);
    }

    public override void Next()
    {

        //Gather Analytics

        if (!_activeToggle)
        {
            _activeToggle = _toggleGroup.GetFirstActiveToggle();
        }

        analyticsService.Track("LeadSourceSelected", new Dictionary<string, object>
        {
            { "currentScreen", ScreenName},
            { "toggleSelected", _activeToggle.GetComponentInChildren<TMP_Text>().text},
            { "timesToggleChanged",_timesToggleChanged}
        });

        //Based on user's cohort, load the appropriate UI

        var configService = ServiceLocator.Get<IConfigService>();
        var variantConfig = configService.GetValue<Dictionary<string, object>>(Bootstrapper.AssignedCohort.ToString(), null);
        SuscriptionScreen newUIScreen;
        switch (Bootstrapper.AssignedCohort)
        {
            case Bootstrapper.Cohort.VariantA:
                newUIScreen = Instantiate(_freeTrailUI, transform.parent).GetComponent<SuscriptionScreen>();
                newUIScreen.InitializeScreen(variantConfig);
                break;

            case Bootstrapper.Cohort.VariantB:
            case Bootstrapper.Cohort.VariantC:
                newUIScreen = Instantiate(_payWallUI, transform.parent).GetComponent<SuscriptionScreen>();
                newUIScreen.InitializeScreen(variantConfig);
                break;
        }

        DestroyImmediate(gameObject);
    }

    public void OnToggleClicked()
    {
        _activeToggle = _toggleGroup
            .ActiveToggles()
            .FirstOrDefault();

        if (!_activeToggle) return;
        
        var toggleText = _activeToggle.GetComponentInChildren<TMP_Text>().text;
        _timesToggleChanged++;
        Debug.Log($"Lead Source is: {toggleText}");
    }
}