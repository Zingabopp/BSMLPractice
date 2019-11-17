using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using VRUI;
using BeatSaberMarkupLanguage;
using System.IO;
using BSMLPractice.UI;

namespace BSMLPractice.Views
{
    public class ExampleViewLeft : HotReloadableViewController
    {
        public override string ResourceName => BSMLNames.ExampleViewLeft;
        public override string ResourceFilePath => @"C:\Users\Jared\source\repos\BSMLPractice\BSMLPractice\Views\ExampleViewLeft.bsml";

        [UIComponent("some-text")]
        private TextMeshProUGUI text;

        [UIAction("pressed")]
        private void TestButtonPressed()
        {
            ContentChanged = true;
            text.text = "Left Test Button Pressed!";
            OnTestPressed?.Invoke();
        }

        [UIAction("back-pressed")]
        private void BackButtonPressed()
        {
            OnBackPressed?.Invoke();
        }

        /// <summary>
        /// Destroy any IDisposable assets here.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public event Action OnBackPressed;
        public event Action OnTestPressed;
    }
}
