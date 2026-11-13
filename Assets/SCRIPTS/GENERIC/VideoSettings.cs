using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    [Header("Visual Config")]
    [SerializeField] private TextMeshProUGUI fpsText;

    [Header("Options")]
    [SerializeField] private Toggle fpsButton;
    [SerializeField] private Toggle screenButton;
    [SerializeField] private Toggle vsyncButton;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private TMP_Dropdown fpsLimiterDropDown;
    Resolution[] resolutions;
    private float deltaTime;
    SettingsManager settingsManager;
    private void Start()
    {
        settingsManager = SettingsManager.Instance;

        CheckResolution();
        InitializeSettings();
    }
    private void Update()
    {
        if (!settingsManager.showFps) return;
        
        deltaTime = Mathf.Lerp(deltaTime, Time.unscaledDeltaTime, 0.1f);

        if (deltaTime > 0)
        {
            float fps = 1f / deltaTime;
            fpsText.text = $"{Mathf.RoundToInt(fps)} FPS";
        }
    }
    private void InitializeSettings()
    {
        fpsButton.isOn = settingsManager.showFps;
        screenButton.isOn = settingsManager.windowed;
        vsyncButton.isOn = settingsManager.vsync;

        fpsText.gameObject.SetActive(settingsManager.showFps);

        fpsLimiterDropDown.value = settingsManager.fpsIndex;
        fpsLimiterDropDown.RefreshShownValue();
        ApplyVSync();
    }


    public void FPSToggleChange()
    {
        settingsManager.showFps = fpsButton.isOn;
        fpsText.gameObject.SetActive(settingsManager.showFps);
    }

    public void ScreenToggleChange()
    {
        if (screenButton.isOn)
        {
            
            Screen.fullScreenMode = FullScreenMode.Windowed;

        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        settingsManager.windowed = screenButton.isOn;
    }

    public void ApplyVSync()
    {
        QualitySettings.vSyncCount = settingsManager.vsync ? 1 : 0;

        Application.targetFrameRate = settingsManager.vsync
            ? -1
            : settingsManager.targetFPS;
    }

    public void CheckResolution()
    {
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();
        List<Resolution> uniqueResolutions = new List<Resolution>();
        HashSet<string> seen = new HashSet<string>();
        int actualResolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string key = resolutions[i].width + "x" + resolutions[i].height;

            if (seen.Contains(key)) continue;
            seen.Add(key);

            uniqueResolutions.Add(resolutions[i]);

            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                actualResolution = uniqueResolutions.Count - 1;
            }
        }
        resolutions = uniqueResolutions.ToArray();

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = uniqueResolutions.Count - 1;
        settingsManager.resolutionIndex = uniqueResolutions.Count - 1;
        resolutionsDropdown.RefreshShownValue();
        //resolutionsDropdown.value = PlayerPrefs.GetInt("resolution", 0);
    }

    public void ChangeResolution(int resolutionIndex)
    {
        //PlayerPrefs.SetInt("resolution", resolutionsDropdown.value);
        Debug.Log(resolutionIndex);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        settingsManager.resolutionIndex = resolutionIndex;
    }

    private readonly int[] fpsOptions = { 30, 60, 120, -1 };

    public void ChangeFpsLimiter()
    {
        int index = fpsLimiterDropDown.value;

        if (index < 0 || index >= fpsOptions.Length) return;
        settingsManager.fpsIndex = index;
        settingsManager.targetFPS = fpsOptions[index];

        ApplyVSync();
    }
}
