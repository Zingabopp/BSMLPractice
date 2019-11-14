using BeatSaberMarkupLanguage.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace BSMLPractice.Views
{
    internal class BSMLSettingsView : PersistentSingleton<BSMLSettingsView>
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
                if (value >= 0 && value <= 10)
                    Config.ExampleIntSetting = value;
            }
        }

        [UIValue("stopCoroutine")]
        public bool StopCoroutineBool
        {
            get { return !Plugin.ExampleGameplayBoolSetting; }
            set { Plugin.ExampleGameplayBoolSetting = !value; }
        }

        [UIValue("exampleString")]
        public string StringExample
        {
            get { return Config.ExampleStringSetting; }
            set { Config.ExampleStringSetting = value; }
        }


        
        [UIValue("list-options")]
        public List<object> ListChoices { get { return PluginConfig.ListChoices; } }

        /// <summary>
        /// value must be a field, not a property.
        /// </summary>
        [UIValue("list-value")]
        private string ListValue = (string)PluginConfig.ListChoices[Config.ExampleListSetting];

        [UIAction("list-OnChange")]
        private void List_OnChange(object obj)
        {
            int index = 0;
            try
            {
                index = ListChoices.FindIndex(o => o == obj);

            }
            catch (Exception ex)
            {
                Logger.log?.Warn($"Error in TextSegment_Apply: {ex.Message}");
            }
            Config.ExampleListSetting = index;
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