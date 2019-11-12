using BeatSaberMarkupLanguage.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace BSMLPractice.UI
{
    internal class BSMLExampleSettings_UI : PersistentSingleton<BSMLExampleSettings_UI>
    {
        private static PluginConfig Config => Plugin.config.Value;
        private void Awake()
        {

        }

        [UIValue("exampleBool")]
        public bool ExampleBool
        {
            get { return Config.ExampleBoolSetting; }
            set { Config.ExampleBoolSetting = value; }
        }

        [UIValue("exampleInt")]
        public int ExampleInt
        {
            get { return Config.ExampleIntSetting; }
            set 
            { 
                if(value >= 0 && value <= 10)
                    Config.ExampleIntSetting = value; 
            }
        }

        [UIValue("stopCoroutine")]
        public bool StopCoroutineBool
        {
            get { return !Plugin.ExampleGameplayBoolSetting; }
            set { Plugin.ExampleGameplayBoolSetting = !value; }
        }


        private static List<object> options = new List<object>() { "ex1", "ex2", "ex3" };
        [UIValue("textSegment-options")]
        public List<object> textSegmentOptions = options;

        [UIValue("textSegment-value")]
        private string TextSegmentsExample = (string)options[Config.ExampleTextSegment];

        [UIAction("textSegment-apply")]
        private void TextSegment_Apply(object obj)
        {
            int index = 0;
            try
            {
                index = textSegmentOptions.FindIndex(o => o == obj);
                
            }
            catch(Exception ex)
            {
                Logger.log?.Warn($"Error in TextSegment_Apply: {ex.Message}");
            }
            Config.ExampleTextSegment = index;
        }

        /// <summary>
        /// Called when the 'Apply' or 'Ok' button is clicked in the settings menu.
        /// </summary>
        [UIAction("#apply")]
        private void ApplyClicked()
        {
            Logger.log.Warn("ApplyClicked");
            Plugin.configProvider.Store(Plugin.config.Value);
        }

        /// <summary>
        /// Called when the 'Cancel' button is clicked in the settings menu.
        /// </summary>
        [UIAction("#cancel")]
        private void CancelClicked()
        {
            Logger.log.Warn("CancelClicked");
        }


    }
}
//<list-setting>
//    <text>List Example</text>
//    <value>textSegment-value</value>
//    <choices>textSegment-options</choices>
//    <apply-on-change>True</apply-on-change>
//    <on-change>textSegment-apply</on-change>
//  </list-setting>