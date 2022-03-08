using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Dropdown resolutionDropdown;

    [SerializeField] private RectTransform backgroundRect;


    private Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        //Resolution windowSize = new Resolution(Screen.width, Screen.height);
        for (int i = 0; i < resolutions.Length; i++)
        {
            string res = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "hz";
            resolutionOptions.Add(res);
            
            if (resolutions[i].Equals(Screen.currentResolution)) currentResolutionIndex = i;
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolumeMaster(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void SetVolumeGame(float volume) {
        audioMixer.SetFloat("gameVolume", volume);
    }

    public void SetVolumeMusic(float volume) {
        audioMixer.SetFloat("musicVolume", volume);
    }
    
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    
    public void SetResolution (int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        //backgroundRect.sizeDelta = new Vector2(resolution.width, resolution.height);
    }
    
}
