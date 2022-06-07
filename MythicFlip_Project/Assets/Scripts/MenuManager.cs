using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Settings")] 
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject menuMain;
    [SerializeField] private GameObject menuSettings;
    [SerializeField] private GameObject bottomResume;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private AudioMixerGroup mixerGroupMusic;
    [SerializeField] private AudioMixerGroup mixerGroupSound;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;

    private void Start()
    {
        Application.targetFrameRate = 60;
        
        EventManager.GamerOver += GameOver;
        panelMenu.SetActive(false);
        
        if (PlayerPrefs.GetInt("EnableMusic") == 1)
            sliderMusic.value = PlayerPrefs.GetFloat("VolumeMusic", -20);
        else
            ToggleMusic(false);
        
        if (PlayerPrefs.GetInt("EnableSound") == 1)
            sliderSound.value = PlayerPrefs.GetFloat("VolumeSound",-20);
        else
            ToggleSound(false);
        
        gameMenu.SetActive(true);
    }
    private void OnDestroy()
    {
        EventManager.GamerOver -= GameOver;;
    }
    private void GameOver()
    {
        gameMenu.SetActive(false);
        menuMain.SetActive(true);
        menuSettings.SetActive(false);
        gameOver.SetActive(true);
        bottomResume.SetActive(false);
        panelMenu.SetActive(true);
    }
    public void Menu()
    {
        EventManager.OnOpenMenu();
        Time.timeScale = 0;
        bottomResume.SetActive(true);
        gameMenu.SetActive(false);
        gameOver.SetActive(false);
        menuMain.SetActive(true);
        menuSettings.SetActive(false);
        panelMenu.SetActive(true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ResumeGame()
    {
        EventManager.OnCloseMenu();
        panelMenu.SetActive(false);
        gameMenu.SetActive(true);
        Time.timeScale = 1;
    }
    public void Settings()
    {
        menuMain.SetActive(false);
        menuSettings.SetActive(true);   
    }
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        if (menuSettings.activeSelf)
        {
            menuSettings.SetActive(false);
            menuMain.SetActive(true);
        }
        else if(menuMain.activeSelf)
            Application.Quit();
    }
    public void ToggleMusic(bool enable)
    {
        if (enable)
            mixerGroupMusic.audioMixer.SetFloat("VolumeMusic", PlayerPrefs.GetFloat("VolumeMusic"));
        else
            mixerGroupMusic.audioMixer.SetFloat("VolumeMusic", -80);
        
        PlayerPrefs.SetInt("EnableMusic", enable ? 1 : 0 );    
    }
    public void ToggleSound(bool enable)
    {
        if (enable)
            mixerGroupSound.audioMixer.SetFloat("VolumeSound",PlayerPrefs.GetFloat("VolumeSound"));
        else
            mixerGroupSound.audioMixer.SetFloat("VolumeSound", -80);
        
        PlayerPrefs.SetInt("EnableSound", enable ? 1 : 0 );
    }
    public void ChangeVolumeMusic(float volume)
    {
        if(PlayerPrefs.GetInt("EnableMusic") == 1)
            mixerGroupMusic.audioMixer.SetFloat("VolumeMusic", volume);
        
        PlayerPrefs.SetFloat("VolumeMusic", volume);
    }
    public void ChangeVolumeSound(float volume)
    {
        if (PlayerPrefs.GetInt("EnableSound") == 1)
            mixerGroupSound.audioMixer.SetFloat("VolumeSound", volume);
            
        PlayerPrefs.SetFloat("VolumeSound", volume);
    }
}

