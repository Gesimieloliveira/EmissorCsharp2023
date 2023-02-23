using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace FusionWPF.Utils
{
    public static class DataGridUtils
    {
        public static void FocusFirstItem(this DataGrid dataGrid)
        {
            try
            {
                dataGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                var row = (DataGridRow) dataGrid.ItemContainerGenerator.ContainerFromIndex(0);

                row.IsSelected = true;
                row.Focus();
            }
            catch (Exception)
            {
                // ingore
            }
        }
    }
}