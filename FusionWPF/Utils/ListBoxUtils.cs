using System.Windows.Controls;
using NHibernate.Util;

namespace FusionWPF.Utils
{
    public static class ListBoxUtils
    {
        public static ListBoxItem FindFirstItem(this ListBox listBox)
        {
            try
            {
                return (ListBoxItem) listBox.ItemContainerGenerator.ContainerFromIndex(0);
            }
            catch
            {
                return null;
            }
        }

        public static void FocusFirstItem(this ListBox listBox)
        {
            if (!listBox.Items.Any())
            {
                return;
            }

            var firstRow = (ListBoxItem) listBox.ItemContainerGenerator.ContainerFromIndex(0);

            listBox.ScrollIntoView(firstRow);

            firstRow.IsSelected = true;
            firstRow.Focus();
        }
    }
}