using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Notify;
//using BSMLPractice.Notify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace BSMLPractice.Views
{
    internal class BSMLSettingsView : PersistentSingleton<BSMLSettingsView>, INotifiableHost
    {
        private static PluginConfig Config => Plugin.config.Value;
        private void Awake()
        {

        }

        [UIAction("openExampleView")]
        public void OpenExampleView()
        {
            Plugin.OnClick();
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
        CustomListTableData data;
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

        public void OnEnable()
        {
            StartCoroutine(IncrementThing());
        }

        private int _thing;
        [UIValue("thing")]
        public int Thing
        {
            get { return _thing; }
            set
            {
                if (_thing == value)
                    return;
                _thing = value;
                NotifyPropertyChanged();
            }
        }

        public IEnumerator<WaitForSeconds> IncrementThing()
        {
            var wait = new WaitForSeconds(5);
            while (true)
            {
                Thing++;
                yield return wait;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            try
            {
                if (PropertyChanged != null)
                {
                    Logger.log?.Critical($"NotifyingPropertyChanged: {propertyName}");
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                {
                    Logger.log?.Critical($"No subscribers for changed property: {propertyName}");
                }
            }
            catch (Exception ex)
            {
                Logger.log?.Error($"Error Invoking PropertyChanged: {ex.Message}");
                Logger.log?.Error(ex);
            }
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