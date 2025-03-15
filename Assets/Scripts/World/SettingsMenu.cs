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
        // ����������� ����������� ������ ���������
        InitializeResolutionDropdown();

        // ����������� ����������� ������ �����
        InitializeQualityDropdown();

        // ������������ ���������� �����������
        LoadSettings();

        // ϳ������ �� ���� �������
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        qualityDropdown.onValueChanged.AddListener(SetQuality);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    // ����������� ����������� ������ ���������
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

    // ����������� ����������� ������ �����
    private void InitializeQualityDropdown()
    {
        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }

    // ������������ ����������
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // ������������ �����
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    // ������������ �������������� ������
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // ���������� �����������
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySetting", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionSetting", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenSetting", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save(); // ���������� ����������� �� ����
    }

    // ������������ �����������
    public void LoadSettings()
    {
        // ������������ �����
        if (PlayerPrefs.HasKey("QualitySetting"))
        {
            int qualityIndex = PlayerPrefs.GetInt("QualitySetting");
            qualityDropdown.value = qualityIndex;
            SetQuality(qualityIndex);
        }

        // ������������ ����������
        if (PlayerPrefs.HasKey("ResolutionSetting"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("ResolutionSetting");
            resolutionDropdown.value = resolutionIndex;
            SetResolution(resolutionIndex);
        }

        // ������������ �������������� ������
        if (PlayerPrefs.HasKey("FullscreenSetting"))
        {
            bool isFullscreen = PlayerPrefs.GetInt("FullscreenSetting") == 1;
            fullscreenToggle.isOn = isFullscreen;
            SetFullscreen(isFullscreen);
        }
    }

    // ����������� ��� ������� ���� �����������
    public void OnSettingsClosed()
    {
        SaveSettings(); // �������� ������������ ����� ���������
    }
}