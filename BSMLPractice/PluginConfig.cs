using System.Collections.Generic;

namespace BSMLPractice
{
    internal class PluginConfig
    {
        public bool RegenerateConfig = true;
        public bool ExampleBoolSetting = false;
        public int ExampleIntSetting = 5;
        public float[] ExampleColorSetting = { 0, 0, 1, 1 };
        public int ExampleTextSegment = 0; // Index from the string array
        public string ExampleStringSetting = "example";
        public float ExampleSliderSetting = 2f;

        public static List<object> ListChoices = new List<object>() { "ex1", "ex2", "ex3" };
        private int _exampleListSetting;
        /// <summary>
        /// Make sure the value isn't out of range. <see cref="BSMLPractice.Views.BSMLSettingsView.ListChoices"/>.
        /// </summary>
        public int ExampleListSetting
        {
            get { return _exampleListSetting; }
            set
            {
                if (value >= 0 && value < ListChoices.Count)
                    _exampleListSetting = value;
                else
                    Logger.log?.Warn($"Example list setting value of {value} is out of range.");
            }
        }
    }
}
