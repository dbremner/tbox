using System.Windows.Forms;

namespace WFControls.Components.DataManagers
{
    public partial class ComboBoxMan : UserControl
    {
        private readonly ItemsMan m_itemsMan = null;

        public ComboBoxMan()
        {
            InitializeComponent();
            m_itemsMan = new ItemsMan(new ComboBoxItemsWorker(cbValues),
                btnAdd, btnEdit, btnDel, null);
        }
        public ItemsMan Items { get { return m_itemsMan; } }
    }
}
