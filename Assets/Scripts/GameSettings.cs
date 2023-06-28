using System.Collections.Generic;
using Cyan;
using Game;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    private const string musicVolumeParameter = "musicVolume";
    private const string sfxVolumeParameter = "sfxVolume";
    private const string SETTINGS_DATA = "SETTINGS_DATA";
    private const float maxSliderValue = 15f;
        
    [SerializeField] private new Renderer2DData renderer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private MenuToggle postFXToggle;
    
    private GameSettingsData data;
    
    private void Awake()
    {
        if (PlayerPrefs.HasKey(SETTINGS_DATA))
        {
            string json = PlayerPrefs.GetString(SETTINGS_DATA);
            data = JsonUtility.FromJson<GameSettingsData>(json);
        }
        else
            data = new GameSettingsData();

        musicGroup.audioMixer.SetFloat(musicVolumeParameter, maxSliderValue * GetVolumeFromSlider(data.musicVolume));
        sfxGroup.audioMixer.SetFloat(sfxVolumeParameter, maxSliderValue * GetVolumeFromSlider(data.sfxVolume));
        TogglePostFX(data.postFX);

        musicSlider.value = data.musicVolume;
        sfxSlider.value = data.sfxVolume;
        postFXToggle.SetValue(data.postFX);
    }

    private void OnDestroy()
    {
        SaveData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SETTINGS_DATA, json);
    }

    public void TogglePostFX(bool value)
    {
        data.postFX = value;
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = value;
        List<ScriptableRendererFeature> features = renderer.rendererFeatures;
        foreach (ScriptableRendererFeature feature in features)
            if (feature is Blit blit)
                blit.SetActive(value);
    }

    public void SetMusicVolume(float value)
    {
        value = GetVolumeFromSlider(value);
        data.musicVolume = value;
        musicGroup.audioMixer.SetFloat(musicVolumeParameter, value);
    }

    public void SetSFXVolume(float value)
    {
        value = GetVolumeFromSlider(value);
        data.sfxVolume = value;
        sfxGroup.audioMixer.SetFloat(sfxVolumeParameter, value);
    }

    private float GetVolumeFromSlider(float value)
    {
        value /= maxSliderValue;
        return Mathf.Log10(Mathf.Lerp(0.0001f, 1f, value)) * 30f;
    }
}
