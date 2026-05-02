using PharmoSys.ViewModels.POS;
using System.Windows.Controls;

namespace PharmoSys.Views.POS
{
    public partial class POSView : UserControl
    {
        public POSView()
        {
            InitializeComponent();
        }

        private void CartDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // After user finishes editing Qty, tell the ViewModel to recalculate the total
            if (DataContext is POSViewModel vm)
            {
                // Commit the edit first, then recalculate
                CartDataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                vm.RecalculateTotal();
            }
        }
    }
}

