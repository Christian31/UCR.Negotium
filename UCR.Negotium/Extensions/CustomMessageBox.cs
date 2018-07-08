using UCR.Negotium.Dialogs;

namespace UCR.Negotium.Extensions
{
    public static class CustomMessageBox
    {
        public static bool Show(string mensaje, string titulo="")
        {
            if (titulo == "")
                titulo = "Confirmar";

            ConfirmationMessage dialog = new ConfirmationMessage(titulo, mensaje);
            dialog.ShowDialog();
            bool result = dialog.Resultado;

            return result;
        }
    }
}
