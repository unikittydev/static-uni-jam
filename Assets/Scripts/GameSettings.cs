using System.Collections.Generic;
using Cyan;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

public class GameSettings : MonoBehaviour
{
    private const float maxSliderValue = 15f;
        
    [SerializeField] private new Renderer2DData renderer;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    private const string musicVolumeParameter = "musicVolume";
    private const string sfxVolumeParameter = "sfxVolume";
    
    public void TogglePostFX(bool value)
    {
        Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = value;
        List<ScriptableRendererFeature> features = renderer.rendererFeatures;
        foreach (ScriptableRendererFeature feature in features)
            if (feature is Blit blit)
                blit.SetActive(value);
    }

    public void SetMusicVolume(float value)
    {
        value = GetVolumeFromSlider(value);
        musicGroup.audioMixer.SetFloat(musicVolumeParameter, value);
    }

    public void SetSFXVolume(float value)
    {
        value = GetVolumeFromSlider(value);
        sfxGroup.audioMixer.SetFloat(sfxVolumeParameter, value);
    }

    private float GetVolumeFromSlider(float value)
    {
        value /= maxSliderValue;
        return Mathf.Log10(Mathf.Lerp(0.0001f, 1f, value)) * 30f;
    }
}
