﻿using System.IO;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.Interface.Atrributes;
using Mnk.TBox.Locales.Localization.Plugins.LocalizationTool;
using Mnk.Library.WPFControls.Dialogs.StateSaver;
using Mnk.Library.WPFSyntaxHighlighter;
using Mnk.TBox.Plugins.LocalizationTool.Code.Settings;
using Mnk.TBox.Plugins.LocalizationTool.Components;

namespace Mnk.TBox.Plugins.LocalizationTool
{
    [PluginInfo(typeof(LocalizationToolLang), typeof(Properties.Resources), PluginGroup.Development)]
    public sealed class LocalizationTool : SingleDialogConfigurablePlugin<Settings, Config, Translate>
    {
        public LocalizationTool() : base(LocalizationToolLang.GroupTranslate)
        {
        }

        protected override void ShowDialog()
        {
            Dialog.Do(Context.DoSync, d => d.ShowDialog(Config), Config.States);
        }

        public override void Init(IPluginContext context)
        {
            base.Init(context);
            context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
        }
    }
}
