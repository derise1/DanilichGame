using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public GameObject menu;
    public GameObject settingsMenu;
    public GameObject levelOver;
    public GameObject instruction;

    public AudioSource audioSource;
    public AudioClip open;
    public int level;

    Resolution[] resolutions;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        if(!PlayerPrefs.HasKey("Last_Level")) PlayerPrefs.SetInt("Last_Level", 1);
        audioSource = GetComponent<AudioSource>();
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    public void ReverseLevel(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(level + 1);
    }

    public void LoadMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void ContinueGame(){
        SceneManager.LoadScene(PlayerPrefs.GetInt("Last_Level"));
    }

    public void Play() {
        SceneManager.LoadScene(1);
        audioSource.PlayOneShot(open);
    }

    public void SetActiveSettings(){
        menu.SetActive(false);
        settingsMenu.SetActive(true);
        audioSource.PlayOneShot(open);
    }

    public void SetActiveInstruction(){
        menu.SetActive(false);
        instruction.SetActive(true);
        audioSource.PlayOneShot(open);
    }

    public void Back(){
        menu.SetActive(true);
        settingsMenu.SetActive(false);
        instruction.SetActive(false);
        audioSource.PlayOneShot(open);
    }

    public void Exit(){
        Application.Quit();
        audioSource.PlayOneShot(open);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        audioSource.PlayOneShot(open);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void ExitSettings()
    {
        SceneManager.LoadScene(1);
        audioSource.PlayOneShot(open);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        audioSource.PlayOneShot(open);
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 3;

        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;
    }
}
