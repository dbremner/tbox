using System;
using System.Windows.Forms;

namespace WFControls.OS
{
    public static class Mt
    {
        public static void Do(Control ctrl, Action a)
        {
            if (ctrl.InvokeRequired) ctrl.BeginInvoke(a);
            else a();
        }
        public static void SetText(Control ctrl, string text) { Do(ctrl, () => { ctrl.Text = text; }); }
        public static void SetEnabled(Control ctrl, bool state) { Do(ctrl, () => { ctrl.Enabled = state; }); }
        public static void SetProgress(ProgressBar pb, int value) { Do(pb, () => { pb.Value = value; }); }
    }
}
