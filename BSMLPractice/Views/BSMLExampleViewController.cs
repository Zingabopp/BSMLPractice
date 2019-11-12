﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace BSMLPractice.Views
{
    public class BSMLExampleViewController : BSMLResourceViewController
    {
        public override string ResourceName => "BSMLPractice.Views.ExampleView.bsml";

        [UIComponent("some-text")]
        private TextMeshProUGUI text;

        [UIAction("press")]
        private void ButtonPress()
        {
            text.text = "Hey look, the text changed";
        }
    }
}