using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace UCR.Negotium.Extensions
{
    public class DialogWithDataGrid : MetroWindow, INotifyPropertyChanged
    {
        protected const string CAMPOREQUERIDO = "Este campo es requerido";
        private string valorACopiar;

        protected double NumeroACopiar
        {
            get
            {
                double numeroConvertido = 0;
                double.TryParse(valorACopiar, out numeroConvertido);
                return numeroConvertido;
            }
        }

        public string ValorACopiar
        {
            get
            {
                return valorACopiar;
            }
            set
            {
                valorACopiar = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ValorACopiar"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected bool ContextMenuDisponible(DependencyObject depObject, IList<DataGridCellInfo> selectedCells)
        {
            bool contextMenuDisponible = false;
            while ((depObject != null) && !(depObject is DataGridCell) && !(depObject is DataGridColumnHeader))
            {
                depObject = VisualTreeHelper.GetParent(depObject);
            }

            if (depObject == null)
                return false;

            if (depObject is DataGridColumnHeader)
            {
                contextMenuDisponible = false;
            }
            else
            {
                var selectedCell = (DataGridCell)depObject;
                if (selectedCell.IsSelected)
                {
                    var selectedCellsDisc = selectedCells.Select(cell => cell.Column.DisplayIndex).Distinct().ToList();
                    if (selectedCellsDisc.Count() != 1 || selectedCellsDisc.Contains(0))
                    {
                        contextMenuDisponible = false;
                    }
                    else
                    {
                        contextMenuDisponible = true;
                    }
                }
                else
                {
                    contextMenuDisponible = false;
                }
            }

            if (contextMenuDisponible)
                ValorACopiar = "0.00";

            return contextMenuDisponible;
        }
    }
}
