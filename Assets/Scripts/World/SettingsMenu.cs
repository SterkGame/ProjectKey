using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;

    void Start()
    {
        // Ініціалізація випадаючого списку розширень
        InitializeResolutionDropdown();

        // Ініціалізація випадаючого списку якості
        InitializeQualityDropdown();

        // Завантаження збережених налаштувань
        LoadSettings();

        // Підписка на зміну значень
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    // Ініціалізація випадаючого списку розширень
    private void InitializeResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions;
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRateRatio.value + "Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Ініціалізація випадаючого списку якості
    private void InitializeQualityDropdown()
    {
        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    // Встановлення розширення
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Встановлення якості
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // Встановлення повноекранного режиму
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Збереження налаштувань
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySetting", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionSetting", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenSetting", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save(); // Збереження налаштувань на диск
    }

    // Завантаження налаштувань
    public void LoadSettings()
    {
        // Завантаження якості
        if (PlayerPrefs.HasKey("QualitySetting"))
        {
            int qualityIndex = PlayerPrefs.GetInt("QualitySetting");
            qualityDropdown.value = qualityIndex;
            SetQuality(qualityIndex);
        }

        // Завантаження розширення
        if (PlayerPrefs.HasKey("ResolutionSetting"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("ResolutionSetting");
            resolutionDropdown.value = resolutionIndex;
            SetResolution(resolutionIndex);
        }

        // Завантаження повноекранного режиму
        if (PlayerPrefs.HasKey("FullscreenSetting"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("FullscreenSetting") == 1;
            fullscreenToggle.isOn = isFullscreen;
            SetFullscreen(isFullscreen);
        }
    }

    // Викликається при закритті меню налаштувань
    public void OnSettingsClosed()
    {
        SaveSettings(); // Зберегти налаштування перед закриттям
    }
}