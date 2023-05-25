using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InicioSesion.Views
{
    /// <summary>
    /// Lógica de interacción para CorreoView.xaml
    /// </summary>
    public partial class CorreoView : UserControl
    {
        public CorreoView()
        {
            InitializeComponent();
        }

        private void btnEnviarCorreo_Click(object sender, RoutedEventArgs e)
        {
            //string Error = "";
            //StringBuilder stringBuilder = new();
            //stringBuilder.Append(txtMensaje.Text.Trim());
            //EnviarCorreo(stringBuilder, DateTime.Now, txtDe.Text, txtPara.Text, txtAsunto.Text,out Error);
        }
        //public void EnviarCorreo(StringBuilder mensaje, DateTime fecha, string De, string Para, string Asunto, out string Error)
        //{
        //    Error = "";
        //    try
        //    {
        //        mensaje.Append(Environment.NewLine);
        //        mensaje.Append(string.Format("Este mensaje ha sido enviado el dia {0:dd/MM/yyyy} a las {0:HH:mm:ss} Hrs.", fecha));
        //        mensaje.Append(Environment.NewLine);

        //        MailMessage mail = new();
        //        mail.From = new MailAddress(De);
        //        mail.To.Add(Para);
        //        mail.Subject = Asunto;
        //        mail.Body = mensaje.ToString();

        //        SmtpClient smtp = new SmtpClient("smtp.office365.com");
        //        smtp.Port = 587;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new NetworkCredential(correo, contraseña);
        //        smtp.EnableSsl = true;
        //        smtp.Send(mail);
        //        Error = "Exito";
        //        MessageBox.Show(Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        Error = "Error: " + ex.Message;
        //        MessageBox.Show(Error);
        //        throw;
        //    }

        //}
    }
}
