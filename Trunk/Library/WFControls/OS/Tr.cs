using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace WFControls.OS
{
    /*
     * Упрощение локализации
     */
    public static class Tr
    {
        public static string DefaultCultureId = "en-US";
        public static SortedList<string, string> Cultures =
            new SortedList<string, string> { 
			{DefaultCultureId,	"English"},
			{"ru-RU",			"Русский"},
			};
        public static string[] culturesNames
        {
            get { return (from g in Cultures select g.Value).ToArray(); }
        }
        public static string[] culturesID
        {
            get { return (from g in Cultures select g.Key).ToArray(); }
        }

        public static CultureInfo ChangeCulture(string langId)
        {
            return Thread.CurrentThread.CurrentUICulture = new CultureInfo(langId);
        }

        public static string GetCultureIDByName(string langName)
        {
            return Cultures.Keys[
                    Math.Max(0, Cultures.IndexOfValue(langName))
                    ];
        }

        public static string GetCultureNameByID(string langId)
        {
            return Cultures.Values[
                    Math.Max(0, Cultures.IndexOfKey(langId))
                    ];
        }

        public static string GetCurrentCultureID()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }

        public static bool IsValidCultureID(string name)
        {
            return (name != null) && (Cultures.IndexOfKey(name) != -1);
        }

        private static void ProcessStatusStrip(Control owner, CultureInfo cultureInfo, ResourceManager resManager)
        {
            var strip = owner as StatusStrip;
            if (strip != null)
            {
                foreach (ToolStripItem item in strip.Items)
                {
                    object text = resManager.GetObject(item.Name + ".Text", cultureInfo);
                    if (text != null)
                        item.Text = (string)text;
                }
            }
        }

        private static bool ProcessComboBox(Control owner, CultureInfo cultureInfo, ResourceManager resManager)
        {
            var cb = owner as ComboBox;
            if (cb != null)
            {
                int count = cb.Items.Count;
                string id = cb.Name + ".Items";
                for (int i = 0; i < count; i++)
                {
                    object text = (i > 0) ?
                        resManager.GetObject(id + i, cultureInfo) :
                        resManager.GetObject(id, cultureInfo);
                    if (text != null)
                        cb.Items[i] = text;
                }
                return true;
            }
            return false;
        }

        private static bool ProcessListView(Control owner, CultureInfo cultureInfo, ResourceManager resManager)
        {
            var lv = owner as ListView;
            if (lv != null)
            {
                int count = lv.Columns.Count;
                string id = lv.Name + "_Column{0}.Text";
                for (int i = 0; i < count; i++)
                {
                    object text = resManager.GetObject(string.Format(id, i + 1), cultureInfo);
                    if (text != null)
                        lv.Columns[i].Text = ((string)text);
                }
                return true;
            }
            return false;
        }

        private static void Localize(string name, Control owner, CultureInfo cultureInfo, ResourceManager resManager)
        {
            object text = resManager.GetObject(name + ".Text", cultureInfo);
            if (text != null)
                owner.Text = (string)text;

            //object location = resManager.GetObject(name + ".Location", cultureInfo);
            //if (location != null) owner.Location = (Point)location;

            if (!ProcessComboBox(owner, cultureInfo, resManager))
            {
                if (!ProcessListView(owner, cultureInfo, resManager))
                {
                    ProcessStatusStrip(owner, cultureInfo, resManager);
                }
            }

            foreach (Control ctrl in owner.Controls)
            {
                Localize(ctrl.Name, ctrl, cultureInfo, resManager);
            }
        }

        private static void Localize(string name, Control owner, CultureInfo cultureInfo, ResourceManager resManager, ToolTip toolTip)
        {
            object tpText = resManager.GetObject(name + ".ToolTip", cultureInfo);
            if (tpText != null)
                toolTip.SetToolTip(owner, (string)tpText);

            foreach (Control ctrl in owner.Controls)
            {
                Localize(ctrl.Name, ctrl, cultureInfo, resManager, toolTip);
            }
        }

        public static void Localize(Form owner, CultureInfo cultureInfo, ToolTip toolTip)
        {
            var resManager = new ResourceManager(owner.GetType());
            const string name = "$this";
            Localize(name, owner, cultureInfo, resManager);
            if (toolTip != null)
                Localize(name, owner, cultureInfo, resManager, toolTip);
        }
    }
}
