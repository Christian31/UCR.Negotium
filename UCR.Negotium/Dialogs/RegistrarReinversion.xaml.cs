﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UCR.Negotium.DataAccess;
using UCR.Negotium.Domain;

namespace UCR.Negotium.Dialogs
{
    /// <summary>
    /// Interaction logic for RegistrarReinversionDialog.xaml
    /// </summary>
    public partial class RegistrarReinversion : MetroWindow, INotifyPropertyChanged
    {
        #region PrivateProperties
        private const string CAMPOREQUERIDO = "Este campo es requerido";
        private const string CAMPOREQUERIDOPOSITIVO = "Este campo es requerido y debe tener un valor mayor a 0";

        private ReinversionData requerimientoReinversionData;
        private InversionData requerimientoInversionData;
        private ProyectoData proyectoData;
        private UnidadMedidaData unidadMedidaData;
        private Reinversion reinversion;
        private List<UnidadMedida> unidadMedidas;
        private Proyecto proyecto;
        private List<Inversion> inversiones;

        private bool vincularInversion;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region Constructor
        public RegistrarReinversion(int codProyecto, int codReinversion = 0)
        {
            InitializeComponent();
            DataContext = this;
            tbDescReinversion.ToolTip = "Ingrese en este campo el Nombre de la Reinversión que desea registrar";
            tbCostoUnitario.ToolTip = "Ingrese en este campo el Costo Unitario de la Reinversión que desea registrar";

            proyecto = new Proyecto();

            requerimientoReinversionData = new ReinversionData();
            requerimientoInversionData = new InversionData();
            proyectoData = new ProyectoData();
            unidadMedidaData = new UnidadMedidaData();

            reinversion = new Reinversion();
            unidadMedidas = new List<UnidadMedida>();
            inversiones = new List<Inversion>();

            proyecto = proyectoData.GetProyecto(codProyecto);
            unidadMedidas = unidadMedidaData.GetUnidadesMedidas();
            inversiones.AddRange(requerimientoInversionData.GetInversiones(codProyecto).Where(inv => inv.Depreciable.Equals(true)).ToList());
            
            reinversion.UnidadMedida = unidadMedidas.FirstOrDefault();
            reinversion.AnoReinversion = AnosDisponibles.FirstOrDefault();
            reinversion.Cantidad = 1;

            if (codReinversion != 0)
            {
                reinversion = requerimientoReinversionData.GetReinversion(codReinversion);
                vincularInversion = !reinversion.CodRequerimientoInversion.Equals(0);
            }
        }
        #endregion

        #region Properties
        public bool VincularInversion
        {
            get
            {
                return vincularInversion;
            }
            set
            {
                vincularInversion = value;
                PropertyChanged(this, new PropertyChangedEventArgs("VincularInversion"));
            }
        }

        public bool Reload { get; set; }

        public List<Inversion> Inversiones
        {
            get
            {
                return inversiones;
            }
            set
            {
                inversiones = value;
            }
        }

        public List<UnidadMedida> UnidadesMedida
        {
            get
            {
                return unidadMedidas;
            }
            set
            {
                unidadMedidas = value;
            }
        }

        public List<int> AnosDisponibles
        {
            get
            {
                List<int> anos = new List<int>();
                for (int i = 1; i <= proyecto.HorizonteEvaluacionEnAnos; i++)
                {
                    int anoActual = proyecto.AnoInicial + i;
                    anos.Add(anoActual);
                }//for

                return anos;
            }
            set
            {
                AnosDisponibles = value;
            }
        } 

        public Reinversion Reinversion
        {
            get
            {
                return reinversion;
            }
            set
            {
                reinversion = value;
            }
        }
        #endregion

        #region Events
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRequiredFields())
            {
                if (!Reinversion.Depreciable)
                    Reinversion.VidaUtil = 0;

                if (!VincularInversion)
                    Reinversion.CodRequerimientoInversion = 0;

                if (Reinversion.CodRequerimientoReinversion.Equals(0))
                {
                    int idInversion = requerimientoReinversionData.InsertarReinversion(Reinversion, proyecto.CodProyecto);
                    if (!idInversion.Equals(-1))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al insertar la reinversión del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (requerimientoReinversionData.EditarReinversion(Reinversion))
                    {
                        //success
                        Reload = true;
                        Close();
                    }
                    else
                    {
                        //error
                        MessageBox.Show("Ha ocurrido un error al actualizar la reinversión del proyecto, verifique que los datos ingresados sean correctos", "Proyecto Actualizado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void tbCostoUnitario_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCostoUnitario.BorderBrush == Brushes.Red)
            {
                tbCostoUnitario.BorderBrush = Brushes.Gray;
                tbCostoUnitario.ToolTip = "Ingrese en este campo el Costo Unitario de la Reinversión que desea registrar";
            }
            int costoUnitario = 0;
            if (!int.TryParse(tbCostoUnitario.Text, out costoUnitario))
            {
                tbCostoUnitario.Text = string.Empty;
            }
        }
        
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void tbDescReinversion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbDescReinversion.BorderBrush == Brushes.Red)
            {
                tbDescReinversion.BorderBrush = Brushes.Gray;
                tbDescReinversion.ToolTip = "Ingrese en este campo el Nombre de la Reinversión que desea registrar";
            }
        }
        #endregion

        #region PrivateMethods
        private bool ValidateRequiredFields()
        {
            bool validationResult = false;
            if (string.IsNullOrWhiteSpace(tbDescReinversion.Text))
            {
                tbDescReinversion.ToolTip = CAMPOREQUERIDO;
                tbDescReinversion.BorderBrush = Brushes.Red;
                validationResult = true;
            }
            if (Convert.ToInt64(tbCostoUnitario.Text.ToString()) <= 0)
            {
                tbCostoUnitario.ToolTip = CAMPOREQUERIDOPOSITIVO;
                tbCostoUnitario.BorderBrush = Brushes.Red;
                validationResult = true;
            }

            return validationResult;
        }
        #endregion

        private void cbxInversiones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbxInversiones.SelectedIndex >= 0)
            {
                Inversion inversion = (Inversion)cbxInversiones.SelectedItem;
                tbDescReinversion.Text = inversion.DescripcionRequerimiento;
                Reinversion.CodRequerimientoInversion = inversion.CodRequerimientoInversion;
            }
        }

        private void cbInversion_Checked(object sender, RoutedEventArgs e)
        {
            cbxInversiones.SelectedIndex = 0;
            PropertyChanged(this, new PropertyChangedEventArgs("Reinversion"));
        }

        private void cbNoInversion_Checked(object sender, RoutedEventArgs e)
        {
            Reinversion.CodRequerimientoInversion = 0;
            Reinversion.DescripcionRequerimiento = "";
            PropertyChanged(this, new PropertyChangedEventArgs("Reinversion"));
        }
    }
}
