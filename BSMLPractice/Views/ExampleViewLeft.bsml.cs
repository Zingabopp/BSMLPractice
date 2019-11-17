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

namespace BSMLPractice.Views
{
    public class ExampleViewLeft : BSMLViewController
    {
        private string _resourceName = BSMLNames.ExampleViewLeft;
        public string _altResourcePath = @"C:\Users\Jared\source\repos\BSMLPractice\BSMLPractice\Views\ExampleViewLeft.bsml";
        private string _content;
        public override string Content
        {
            get
            {
                if (string.IsNullOrEmpty(_content))
                    _content = File.ReadAllText(_altResourcePath);
                return _content;
            }
        }
        private bool _refreshContent;
        public bool RefreshContent {
            get { return _refreshContent; }
            set
            {
                if (value == true)
                    _content = null;
                _refreshContent = value;
            }
        }
        

        [UIComponent("some-text")]
        private TextMeshProUGUI text;

        [UIAction("pressed")]
        private void TestButtonPressed()
        {
            RefreshContent = true;
            _resourceName = BSMLNames.ExampleViewRight;
            text.text = "Left Test Button Pressed!";
            OnTestPressed?.Invoke();
        }

        [UIAction("back-pressed")]
        private void BackButtonPressed()
        {
            OnBackPressed?.Invoke();
        }

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            if (RefreshContent && !firstActivation)
                BSMLParser.instance.Parse(Content, gameObject, this);
            base.DidActivate(firstActivation, type);
            RefreshContent = false;
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
