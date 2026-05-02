using PharmoSys.ViewModels.POS;
using System.Windows.Controls;
using System.Windows.Threading;

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
            // IMPORTANT: Never call CommitEdit inside CellEditEnding — it causes WPF re-entrancy and freezes.
            // Defer via Dispatcher.BeginInvoke so the current edit cycle completes first.
            Dispatcher.BeginInvoke(DispatcherPriority.Background, new System.Action(() =>
            {
                if (DataContext is POSViewModel vm)
                {
                    vm.RecalculateTotal();
                }
            }));
        }
    }
}


