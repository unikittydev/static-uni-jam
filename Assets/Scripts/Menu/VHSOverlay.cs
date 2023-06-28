using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game
{
    public class VHSOverlay : MonoBehaviour
    {
        private const string playWithSymbol = "ВОСПР.⏵";
        private const string stopWithSymbol = "СТОП⏹";
        private const string pauseWithSymbol = "ПАУЗА⏸";
        private const string defaultCountdownText = "--:--";
        
        private const string timeFormat = @"hh\:mm\:ss";
        private const string countdownFormat = @"ss\:ff";

        [SerializeField] private TMP_Text levelName;
        [SerializeField] private TMP_Text levelStateName;
        [SerializeField] private TMP_Text timer;
        [SerializeField] private TMP_Text countdown;
        
        [SerializeField] private GameObject overlay;

        [SerializeField] private float symbolFlickerRate = 1f;
        
        private Coroutine animate;

        public static VHSOverlay Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Play(string levelName, LevelState state)
        {
            overlay.SetActive(true);
            foreach (Transform child in overlay.transform)
                child.gameObject.SetActive(true);
            this.levelName.text = levelName;
            string stateName = state == LevelState.Play
                ? playWithSymbol
                : state == LevelState.Pause ? pauseWithSymbol : stopWithSymbol;
            StartCoroutine(Animate(stateName));
        }

        public void ShowCountdown()
        {
            overlay.SetActive(true);
            foreach (Transform child in overlay.transform)
                child.gameObject.SetActive(false);
            countdown.gameObject.SetActive(true);
        }

        public void UpdateCountdown(float counter)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(counter * 1000f);
            countdown.text = time.ToString(countdownFormat);
        }

        public void ResetCountdown()
        {
            countdown.text = defaultCountdownText;
        }

        public void Stop()
        {
            if (animate != null)
                StopCoroutine(animate);
            overlay.SetActive(false);
        }

        private IEnumerator Animate(string stateName)
        {
            float timerCounter = 1f, flickerCounter = symbolFlickerRate;
            int seconds = 0;
            bool symbolOn = true;
            string stateNameWOSymbol = stateName[..^1];

            levelStateName.text = stateName;
            while (overlay.activeSelf)
            {
                if (timerCounter >= 1f)
                {
                    timer.text = TimeSpan.FromSeconds(seconds).ToString(timeFormat);
                    timerCounter -= 1f;
                    seconds++;
                }

                if (flickerCounter >= symbolFlickerRate)
                {
                    levelStateName.text = symbolOn ? stateName : stateNameWOSymbol;
                    symbolOn = !symbolOn;
                    flickerCounter -= symbolFlickerRate;
                }
                timerCounter += Time.unscaledDeltaTime;
                flickerCounter += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
