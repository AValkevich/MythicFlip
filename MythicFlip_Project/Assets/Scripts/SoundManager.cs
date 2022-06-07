using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [Header("Music Settings")] 
    [SerializeField] private AudioClip[] musicAudioClips;
    
    private AudioSource _backgroundMusic;

    private void Awake()
    {
        _backgroundMusic = GetComponent<AudioSource>();
    }
    private void Start()
    {
        if(SceneManager.sceneCount == 0)
            StartMusic();
        else
            StartLevelMusic();
    }
    private void StartMusic()
    {
        _backgroundMusic.clip = musicAudioClips[0];
        _backgroundMusic.volume = 0;
        _backgroundMusic.Play();
        StartCoroutine(MusicFadeOut());
    }
    private void StartLevelMusic()
    {
        _backgroundMusic.clip = musicAudioClips[Random.Range(0,musicAudioClips.Length)];
        _backgroundMusic.volume = 0;
        _backgroundMusic.Play();
        StartCoroutine(MusicFadeOut());
    }
    private IEnumerator MusicFadeOut()
    {
        while (_backgroundMusic.volume < 1)
        {
            _backgroundMusic.volume += Time.deltaTime * 0.1f;
            yield return new WaitForFixedUpdate();
        }
    }
}
