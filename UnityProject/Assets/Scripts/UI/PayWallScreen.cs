using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PayWallScreen : SuscriptionScreen
{
    [SerializeField] private TextMeshProUGUI _tMPPriceOptionA;
    [SerializeField] private TextMeshProUGUI _tMPPriceOptionB;
    [SerializeField] private OptionSelectorMenu _optionSelectorMenu;
    public override void InitializeScreen(Dictionary<string, object> newScreenConfig)
    {
        base.InitializeScreen(newScreenConfig);

        _tMPPriceOptionA.text = $"${_screenConfiguration.GetValueOrDefault("priceOptionA", "99.99")}/year";
        _tMPPriceOptionB.text = $"${_screenConfiguration.GetValueOrDefault("priceOptionB", "14.99")}/month";
    }

    public override void Next()
    {
        analyticsService.Track("startedFreeTrail", new Dictionary<string, object>
        {
            { "currentScreen", ScreenName},
            {"optionAPrice", _screenConfiguration.GetValueOrDefault("priceOptionA", "99.99")},
            { "optionBPrice", _screenConfiguration.GetValueOrDefault("priceOptionB", "14.99")},
            {"optionSelected", _optionSelectorMenu.CurrentOptionSelected == 0 ? "Option A" : "Option B"},
            {"timesUserChangedOption", _optionSelectorMenu.TimesChangedOption }
        });

        analyticsService.Flush();

        if (_nextUI)
        {
            Instantiate(_nextUI, transform.parent);
            DestroyImmediate(gameObject);
        }
    }
}
