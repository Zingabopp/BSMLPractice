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

        private int _exampleListSetting;
        /// <summary>
        /// Make sure the value isn't out of range. <see cref="BSMLPractice.Views.BSMLSettingsView.ListChoices"/>.
        /// </summary>
        public int ExampleListSetting
        {
            get { return _exampleListSetting; }
            set
            {
                if (value >= 0 && value < 3)
                    _exampleListSetting = value;
                else
                    Logger.log?.Warn($"Example list setting value of {value} is out of range.");
            }
        }
    }
}
