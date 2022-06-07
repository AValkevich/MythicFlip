using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Settings")] 
    [SerializeField] private GameObject menuMain;
    [SerializeField] private GameObject menuSettings;
    [SerializeField] private AudioMixerGroup mixerGroupMusic;
    [SerializeField] private AudioMixerGroup mixerGroupSound;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;
    private void Start()
    {
        Application.targetFrameRate = 60;

        if (PlayerPrefs.GetInt("EnableMusic") == 1)
            sliderMusic.value = PlayerPrefs.GetFloat("VolumeMusic", -20);
        else
            ToggleMusic(false);
        
        if (PlayerPrefs.GetInt("EnableSound") == 1)
            sliderSound.value = PlayerPrefs.GetFloat("VolumeSound",-20);
        else
            ToggleSound(false);
    }
    public void Settings()
    {
        menuMain.SetActive(false);
        menuSettings.SetActive(true);   
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

