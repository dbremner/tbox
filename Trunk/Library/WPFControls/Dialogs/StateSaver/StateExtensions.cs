using System;
using System.Collections.Generic;
using System.Windows;
using Mnk.Library.Common.Tools;
using Mnk.Library.WpfControls.Code;

namespace Mnk.Library.WpfControls.Dialogs.StateSaver
{
    public static class StateExtensions
    {
        public static DialogState GetState(this Window dialog)
        {
            return new DialogState
            {
                Left = dialog.Left,
                Top = dialog.Top,
                Width = dialog.Width,
                Height = dialog.Height,
                WindowState = dialog.WindowState == WindowState.Maximized ?
                                dialog.WindowState : WindowState.Normal,
            };
        }

        public static void SetState(this Window dialog, DialogState state)
        {
            if (state == null) return;
            dialog.Loaded += (o, e) =>
            {
                dialog.Width = state.Width;
                dialog.Height = state.Height;
                dialog.Left = SystemParameters.WorkArea.Left + Math.Max(0,
                    Math.Min(state.Left - SystemParameters.WorkArea.Left, SystemParameters.WorkArea.Width - dialog.Width));
                dialog.Top = SystemParameters.WorkArea.Top + Math.Max(0,
                    Math.Min(state.Top - SystemParameters.WorkArea.Top, SystemParameters.WorkArea.Height - dialog.Height));
                dialog.WindowState = state.WindowState;
            };
        }

        public static void SaveState<T>(this LazyDialog<T> dialog, IDictionary<string, DialogState> items)
            where T : Window, IDisposable
        {
            if (!dialog.IsValueCreated) return;
            items[dialog.Id] = dialog.Value.GetState();
        }

        public static void LoadState<T>(this LazyDialog<T> dialog, IDictionary<string, DialogState> items)
            where T : Window, IDisposable
        {
            if (dialog.IsValueCreated) return;
            dialog.Value.SetState(items.Get(dialog.Id));
        }

        public static void Do<T>(this LazyDialog<T> dialog, Action<Action> syncronizer, Action<T> action, IDictionary<string, DialogState> items)
            where T : DialogWindow
        {
            var isCreated = dialog.IsValueCreated;
            dialog.Do(syncronizer, o =>
            {
                if (!isCreated) dialog.Value.SetState(items.Get(dialog.Id));
                action(o);
            });
        }

        public static void Show<T>(this LazyDialog<T> dialog, Action<Action> syncronizer, IDictionary<string, DialogState> items)
            where T : DialogWindow
        {
            dialog.Do(syncronizer, d => d.ShowAndActivate(), items);
        }
    }
}
