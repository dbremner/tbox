﻿using Mnk.TBox.Locales.Localization.Plugins.Notes;

namespace Mnk.TBox.Plugins.Notes.Code.Localization
{
    public class TrExtension : Mnk.Library.WPFControls.Localization.TranslateExtension
    {
        public TrExtension(string key) : base(key, NotesLang.ResourceManager) { }
    }
}
