using System.Windows;
using System.Windows.Controls;
using Mnk.TBox.Plugins.Notes.Code.Settings;

namespace Mnk.TBox.Plugins.Notes.Components
{
    /// <summary>
    /// Interaction logic for NotesDialog.xaml
    /// </summary>
    public partial class NotesDialog 
    {
        public NotesDialog()
        {
            InitializeComponent();
        }

        public void ShowDialog(Profile p, Window owner)
        {
            Title = p.Key;
            DataContext = p;
            Owner = owner;
            if (owner != null)
            {
                SafeShowDialog();
            }
            else ShowAndActivate();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
