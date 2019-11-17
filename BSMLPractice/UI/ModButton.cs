using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.MenuButtons;

namespace BSMLPractice.UI
{
    public class ModButton : MenuButton
    {
        private string _text;
        private string _hoverHint;
        public override string Text { get { return _text; } }
        public override string HoverHint { get { return _hoverHint; } }

        public override Action OnClick { get { return Click; } }

        private Action _click;
        public Action Click
        {
            get => _click;
            set => _click = value;
        }

        public ModButton(string text, string hoverHint, Action onClick)
        {
            _text = text;
            _hoverHint = hoverHint;
            Click = onClick;
        }
    }
}
