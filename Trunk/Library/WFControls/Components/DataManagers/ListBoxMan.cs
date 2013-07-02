using System.Windows.Forms;

namespace WFControls.Components.DataManagers
{
    public partial class ListBoxMan : UserControl
    {
        private readonly ItemsMan m_itemsMan = null;

        public ListBoxMan()
        {
            InitializeComponent();
            m_itemsMan = new ItemsMan(new ListBoxItemsWorker(lbParams),
                btnAdd, btnEdit, btnDel, btnClear);
        }

        public ItemsMan Items { get { return m_itemsMan; } }
    }
}
