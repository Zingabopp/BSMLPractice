﻿using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Notify;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
//using BSMLPractice.Notify;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BSMLPractice.Views
{
    public class ExampleViewLeft : BSMLPractice.UI.HotReloadableViewController, INotifiableHost
    {
        public void Start()
        {
            Logger.log?.Warn("ExampleViewLeft is Starting.");

        }

        public void OnEnable()
        {
            StartCoroutine(ExampleCoroutine());
        }
        public override string ResourceName => BSMLNames.ExampleViewLeft;
        public override string ContentFilePath => @"C:\Users\Jared\source\repos\BSMLPractice\BSMLPractice\Views\ExampleViewLeft.bsml";

        private string _exampleText;
        [UIValue("ExamplText")]
        public string ExampleText
        {
            get
            {
                return _exampleText ?? string.Empty;
            }
            set
            {
                if (_exampleText == value)
                    return;
                //Logger.log?.Error($"Changing ExampleText to {value}");
                _exampleText = value;
                NotifyPropertyChanged();
            }
        }

        private string _otherText;
        [UIValue("OtherText")]
        public string OtherText
        {
            get
            {
                return _otherText ?? string.Empty;
            }
            set
            {
                if (_otherText == value)
                    return;
                //Logger.log?.Error($"Changing OtherText to {value}");
                _otherText = value;
                NotifyPropertyChanged();
            }
        }

        private string _fontName;
        [UIValue("TextFont")]
        public string FontName
        {
            get
            {
                return _fontName ?? string.Empty;
            }
            set
            {
                if (_fontName == value)
                    return;
                Logger.log?.Error($"Changing FontName to {value}");
                _fontName = value;
                NotifyPropertyChanged();
            }
        }

        private bool _italics;
        [UIValue("Italics")]
        public bool Italics
        {
            get
            { return _italics; }
            set
            {
                if (_italics == value) return;
                _italics = value;
                NotifyPropertyChanged();
            }
        }

        [UIAction("pressed")]
        private void TestButtonPressed()
        {
            ContentChanged = true;
            //text.text = "Left Test Button Pressed!";
            OnTestPressed?.Invoke();
        }

        [UIAction("back-pressed")]
        private void BackButtonPressed()
        {
            OnBackPressed?.Invoke(this);
        }

        [UIValue("GlowColor")]
        public string glowcolor => "#FFFF00FF";

        /// <summary>
        /// Destroy any IDisposable assets here.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public event Action<ViewController> OnBackPressed;
        public event Action OnTestPressed;


        private IEnumerator ExampleCoroutine()
        {
            //Logger.log?.Info($"{name}.ExampleCoroutine(): In ExampleCoroutine().");
            WaitForSeconds exampleWait = new WaitForSeconds(5); // Created here so we can reuse it in the loop.
            WaitUntil waitUntilTrue = new WaitUntil(() => Plugin.ExampleGameplayBoolSetting);
            while (true)
            {
                int count = 1;
                while (Plugin.ExampleGameplayBoolSetting)
                {
                    //Logger.log?.Info($"Count is {count}, ExampleGameplayListSetting: {Plugin.ExampleGameplayListSetting}");
                    ExampleText = $"Example: {count}";
                    OtherText = $"OtherText: {-count}";
                    Italics = !Italics;
                    if (Italics)
                        FontName = "SourceHanSansCN-Bold-SDF-Common-1(2k)";
                    else
                        FontName = "Teko-Medium SDF No Glow";
                    count++;
                    // yield return new WaitForSeconds(5); // Could do this, but it would create extra garbage to recreate the same object every time.
                    yield return exampleWait;
                }
                yield return waitUntilTrue; // Wait until Plugin.ExampleGamePlayBoolSetting is true again.
            }
        }
    }
}
