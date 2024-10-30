using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeButtonManager : MonoBehaviour
{
    public Button timeButton;
    public TextMeshProUGUI buttonText;
    private readonly int[] _timeScales = { 1, 2, 4 };
    [SerializeField] private int _currentScaleIndex = 0;

    private void Start()
    {
        if (timeButton != null)
        {
            timeButton.onClick.AddListener(ChangeTimeScale);
            UpdateButtonText();
        }
    }

    private void ChangeTimeScale()
    {
        _currentScaleIndex = (_currentScaleIndex + 1) % _timeScales.Length;
        Time.timeScale = _timeScales[_currentScaleIndex];
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = _timeScales[_currentScaleIndex] + "x";
        }
    }
}