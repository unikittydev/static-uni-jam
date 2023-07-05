using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GameSettingsData
    {
        public float sfxVolume = GameSettings.maxSliderValue;
        public float musicVolume = GameSettings.maxSliderValue;
        public bool postFX;

        public GameSettingsData()
        {
            postFX = !Application.isMobilePlatform;
        }
    }
}