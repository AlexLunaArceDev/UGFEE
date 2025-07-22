using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeadSourceScreen : UIScreenBase
{
    private ToggleGroup _toggleGroup;
    private Toggle _activeToggle;

    private void Awake()
    {
        _toggleGroup = GetComponentInChildren<ToggleGroup>();
    }

    public override void Next()
    {
        //TODO: Based on user's cohort, load the appropriate UI
        //TODO: Gather Analytics
    }

    public void OnToggleClicked()
    {
        _activeToggle = _toggleGroup.ActiveToggles().FirstOrDefault();
        if (_activeToggle)
        {
            var toggleText = _activeToggle.GetComponentInChildren<TMP_Text>().text;
            Debug.Log($"Lead Source is: {toggleText}");
        }
    }
}