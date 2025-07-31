using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionSelectorMenu : MonoBehaviour
{
    [Serializable]
    public class Option
    {
        public Button button;
        public List<TextMeshProUGUI> texts;
        public List<Image> images;

        public void UpdateColors(Color backgroudColor, Color textColor, Color imageColor)
        {
            button.image.color = backgroudColor;
            foreach (TextMeshProUGUI text in texts)
            {
                text.color = textColor;
            }
            foreach(Image image in images)
            {
                image.color = imageColor;
            }
        }
    }

    [SerializeField]
    private List<Option> _options = new List<Option>();

    [Header("Buttons Colors")]
    [SerializeField]
    private Color _defaultBackgroundColor = Color.white;
    [SerializeField]
    private Color _defaultTextColor = Color.white;
    [SerializeField]
    private Color _defaultImagesColor = Color.white;

    [SerializeField]
    private Color _selectedBackgroundColor = Color.white;
    [SerializeField]
    private Color _selectedTextColor = Color.white;
    [SerializeField]
    private Color _selectedImagesColor = Color.white;


    private int _currentSelectedIndex = -1;
    public int CurrentOptionSelected => _currentSelectedIndex;

    private int _timesUserChangedOption = 0;
    public int TimesChangedOption => _timesUserChangedOption;

    void Start()
    {
        for (int i = 0; i < _options.Count; i++)
        {
            int index = i;
            _options[i].button.onClick.AddListener(() => SelectOption(index));
            _options[i].UpdateColors(_defaultBackgroundColor, _defaultTextColor, _defaultImagesColor);

        }
        _timesUserChangedOption--;
        SelectOption(0);
    }

    private void SelectOption(int newIndexSelected)
    {
        if (newIndexSelected == _currentSelectedIndex) return;

        _timesUserChangedOption++;

        int previousSelected = _currentSelectedIndex;
        _currentSelectedIndex = newIndexSelected;

        if (previousSelected > -1)
        {
            UpdateVisuals(previousSelected, false);
        }
        UpdateVisuals(_currentSelectedIndex, true);
    }

    private void UpdateVisuals(int optionIndex, bool isSelected)
    {
        if (isSelected)
        {
            _options[optionIndex].UpdateColors(_selectedBackgroundColor, _selectedTextColor, _selectedImagesColor);
        }
        else
        {
            _options[optionIndex].UpdateColors(_defaultBackgroundColor, _defaultTextColor, _defaultImagesColor);
        }
    }
}
