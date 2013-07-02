using System.Windows.Forms;

namespace WFControls.Components.DataManagers
{
    public partial class CheckedListBoxMan : UserControl
    {
        private readonly CheckedItemsMan m_itemsMan;

        public CheckedListBoxMan()
        {
            InitializeComponent();
            m_itemsMan = new CheckedItemsMan(new CheckedListBoxItemsWorker(lbParams),
                btnAdd, btnEdit, btnDel, btnClear, btnAll, btnNone);
        }

        public CheckedItemsMan Items { get { return m_itemsMan; } }

    }
}
