using System.Windows;
using UCR.Negotium.Dialogs;

namespace UCR.Negotium.Extensions
{
    public static class CustomMessageBox
    {
        public static bool ShowConfirmationMesage(string mensaje, string titulo= "Confirmar")
        {
            ConfirmationMessage dialog = new ConfirmationMessage(titulo, mensaje);
            dialog.ShowDialog();
            bool result = dialog.Resultado;

            return result;
        }

        public static void ShowGenericError()
        {
            MessageBox.Show("Existen errores en los campos que desea guardar, " +
                    "favor revise las validaciones y complete los datos de manera correcta.", "Error de validación",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
