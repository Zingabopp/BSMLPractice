using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using System.IO;
using BeatSaberMarkupLanguage;

namespace BSMLPractice.Views
{
    public class ExampleViewRight : HotReloadableViewController
    {
        public override string ResourceName => BSMLNames.ExampleViewRight;
        public override string ResourceFilePath => @"C:\Users\Jared\source\repos\BSMLPractice\BSMLPractice\Views\ExampleViewRight.bsml";

        [UIComponent("some-text")]
        private TextMeshProUGUI text;

        [UIAction("pressed")]
        private void TestButtonPressed()
        {
            ContentChanged = true;
            text.text = "Right Test Button Pressed!";
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
