using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Components;
using SongCore;
using HMUI;

namespace BSMLPractice.Views
{
    public class ExampleViewCenter : HotReloadableViewController
    {
        public override string ResourceName => BSMLNames.ExampleViewCenter;
        public override string ResourceFilePath => @"C:\Users\Jared\source\repos\BSMLPractice\BSMLPractice\Views\ExampleViewCenter.bsml";
        [UIComponent("list")]
        public CustomListTableData list;
        public static List<Stuff> StuffList;
        static ExampleViewCenter()
        {
            StuffList = new List<Stuff>();
            for (int i = 0; i < 20; i++)
                StuffList.Add(new Stuff($"Test {i}", $"Sub {i}", $"Mapper {i}"));
        }

        public struct Stuff
        {
            public string SongName;
            public string SongSubName;
            public string Mapper;
            public Stuff(string songName, string songSubname, string mapper)
            {
                SongName = songName;
                SongSubName = songSubname;
                Mapper = mapper;
            }
        }

        private List<CustomListTableData.CustomCellInfo> tableData;

        [UIValue("table-data")]
        public List<CustomListTableData.CustomCellInfo> TableData
        {
            get
            {
                if (tableData == null)
                {
                    tableData = StuffList.Select(s => new CustomListTableData.CustomCellInfo(s.SongName, s.SongSubName)).ToList();
                }
                return tableData;
            }
        }

        [UIComponent("some-text")]
        private TextMeshProUGUI text;

        [UIAction("press")]
        private void ButtonPress()
        {
            text.text = "Hey look, the text changed";
            OnReloadPressed?.Invoke();
        }

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);
            if (list != null)
            {
                list.data = TableData;
                list.tableView.ReloadData();
            }
        }

        public event Action OnReloadPressed;
    }
}
