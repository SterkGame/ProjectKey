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

    public TextMeshProUGUI deathsText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI winsText;

    private Resolution[] availableResolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 1280, height = 720 }
    };

    void Start()
    {
        InitializeResolutionDropdown();
        InitializeQualityDropdown();
        LoadSettings();
        ShowStats();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    private void InitializeResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + "x" + availableResolutions[i].height;
            options.Add(option);

            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void InitializeQualityDropdown()
    {
        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= availableResolutions.Length)
        {
            Debug.LogWarning("Invalid resolution index: " + resolutionIndex);
            return;
        }

        Resolution resolution = availableResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySetting", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionSetting", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenSetting", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("QualitySetting"))
        {
            int qualityIndex = PlayerPrefs.GetInt("QualitySetting");
            qualityDropdown.value = qualityIndex;
            SetQuality(qualityIndex);
        }

        if (PlayerPrefs.HasKey("ResolutionSetting"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("ResolutionSetting");

            if (resolutionIndex < 0 || resolutionIndex >= availableResolutions.Length)
            {
                resolutionIndex = 0;
            }

            resolutionDropdown.value = resolutionIndex;
            SetResolution(resolutionIndex);
        }

        if (PlayerPrefs.HasKey("FullscreenSetting"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("FullscreenSetting") == 1;
            fullscreenToggle.isOn = isFullscreen;
            SetFullscreen(isFullscreen);
        }
    }

    public void ShowStats()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        deathsText.text = "Програв: " + data.totalDeaths;
        killsText.text = "Вбив ворогів: " + data.totalKills;
        winsText.text = "Переміг: " + data.totalWins;
    }

    public void OnSettingsClosed()
    {
        SaveSettings();
    }

    public void ResetProgress()
    {
        SaveSystem.ResetPlayer();
        Debug.Log("Прогрес скинуто до початкових значень!");
        ShowStats();
    }
}
