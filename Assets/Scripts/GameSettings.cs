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
    public const float maxSliderValue = 15f;
        
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
        ApplySettings();
    }

    private void OnDestroy()
    {
        SaveData();
    }

    [ContextMenu("Reset Settings")]
    public void ResetSettings()
    {
        PlayerPrefs.DeleteKey(SETTINGS_DATA);
        data = new GameSettingsData();
        ApplySettings();
    }

    private void ApplySettings()
    {
        musicGroup.audioMixer.SetFloat(musicVolumeParameter, GetVolumeFromSlider(data.musicVolume));
        sfxGroup.audioMixer.SetFloat(sfxVolumeParameter, GetVolumeFromSlider(data.sfxVolume));
        TogglePostFX(data.postFX);

        musicSlider.value = data.musicVolume;
        sfxSlider.value = data.sfxVolume;
        postFXToggle.SetValue(data.postFX);
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
        data.musicVolume = value;
        value = GetVolumeFromSlider(value);
        musicGroup.audioMixer.SetFloat(musicVolumeParameter, value);
    }

    public void SetSFXVolume(float value)
    {
        data.sfxVolume = value;
        value = GetVolumeFromSlider(value);
        sfxGroup.audioMixer.SetFloat(sfxVolumeParameter, value);
    }

    private float GetVolumeFromSlider(float value)
    {
        value /= maxSliderValue;
        return Mathf.Log10(Mathf.Lerp(0.0001f, 1f, value)) * 30f;
    }
}
