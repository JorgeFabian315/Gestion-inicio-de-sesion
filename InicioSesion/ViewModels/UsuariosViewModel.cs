using FluentValidation.Results;
using FluentValidation;
using GalaSoft.MvvmLight.Command;
using InicioSesion.Catalogos;
using InicioSesion.Models;
using InicioSesion.Validaciones;
using InicioSesion.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InicioSesion.ViewModels
{
    public class UsuariosViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Usuarios> ListaUsuarios { get; set; } = new();
        public ObservableCollection<Roles> ListaRoles { get; set; } = new();

        UsuariosCatalogo cusuarios = new();

        RolesCatalogo crol = new();
        public int TotalEmpleados { get; set; }
        public Usuarios Usuario { get; set; } = new();
        public string Modo { get; set; } = "";
        public string Error { get; set; } = "";
        public ICommand GuardarCommand { get; set; }
        public bool VerEliminarBool { get; set; } = false;
        public ICommand CambiarVistaCommand => new RelayCommand<string>(CambiarVista);
        public ICommand VerGuardarCommand => new RelayCommand(VerGuardar);
        public ICommand VerEliminarCommand => new RelayCommand<Usuarios>(VerEliminar);
        public ICommand EliminarCommand => new RelayCommand(Eliminar);
        public ICommand VerEditarCommand => new RelayCommand<string>(VerEditar);
        public ICommand VerEnviarCorreoCommand => new RelayCommand<Usuarios>(VerEnviarCorreo);
        public ICommand CancelarEditarCommand => new RelayCommand<Usuarios>(Cancelar);
        public ICommand VerBitacorasCommand => new RelayCommand<Usuarios>(VerBitacoras);
        public ICommand EnviarCorreoCommand => new RelayCommand<string>(EnviarCorreo);

        private string nombre_viejo = string.Empty;
        public UsuariosViewModel()
        {
            GuardarCommand = new RelayCommand<int>(Guardar);
            GetUsuarios();
            GetRoles();
            Actualizar();
        }

        private void VerBitacoras(Usuarios us)
        {
            Modo = "VerBitacoras";
            if (us is not null)
                Usuario = cusuarios.GetUsuario(us.Correo);
            Actualizar();
        }
        private void VerGuardar()
        {
            Usuario = new();
            Error = "";
            Modo = "Agregar";
            Actualizar();
        }
        private void Cancelar(Usuarios obj)
        {
            cusuarios.Recargar(Usuario);
            Modo = "Ver";
            Actualizar();
        }

        private void VerEditar(string  correo)
        {
            Error = "";

            Usuario = cusuarios.GetUsuario(correo);

            if (Usuario != null)
            {
                Modo = "Editar";
                nombre_viejo = Usuario.Nombre;
            }
            Actualizar();
        }
        private void VerEnviarCorreo(Usuarios obj)
        {
            if (obj != null)
            {
                Usuario = obj;
                Modo = "Correo";
                Actualizar();
            }
        }
        private void Eliminar()
        {
            if (Usuario is not null)
            {
                cusuarios.Eliminar(Usuario);
                GetUsuarios();
                VerEliminarBool = false;
                Actualizar();
            }
        }



        void VerEliminar(Usuarios usuario)
        {
            if (usuario is not null)
            {
                Usuario = usuario;
                VerEliminarBool = true;
            }
            Actualizar();

        }
        public void Guardar(int id)
        {
            if (Usuario is not null)
            {

                UsuarioValidator ususariovalidator = new UsuarioValidator(ListaUsuarios);
                // Recibe el objeto que va a validar y regresa un ValidationResult
                var result = ususariovalidator.Validate(Usuario, option => option.IncludeAllRuleSets());
                // option.IncludeAllRuleSets() sirve para agregar todas las reglas, porque puedes dividirlas en grupos


                Error = "";
                if (result.IsValid)
                {
                    Usuario.IdRol = id;


                    if (Usuario.Id != 0)
                    {

                       cusuarios.Editar(Usuario, nombre_viejo);
                    }
                    else
                    {
                        cusuarios.Agregar(Usuario);
                    }

                    GetUsuarios();
                    Modo = "Ver";
                }

                else
                    foreach (var error in result.Errors)
                        Error = $"{Error} {error} {Environment.NewLine}";

                Actualizar();
            }
        }
        public void CambiarVista(string vista)
        {

            if (vista == "CancelarEliminar")
                VerEliminarBool = false;


            Modo = vista;
            Actualizar();
        }

        public void GetUsuarios()
        {
            ListaUsuarios.Clear();

            foreach (Usuarios usuario in cusuarios.GetUsuarios())
                ListaUsuarios.Add(usuario);
        }
        public void GetRoles()
        {
            ListaRoles.Clear();
            var lista = crol.GetRoles();
            foreach (var rol in lista)
                ListaRoles.Add(rol);
        }
        public void EnviarCorreo(string mensaje)
        {
            Error = "";
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(Usuario.Correo, "Registro en sistema de usuarios"),
                    Subject = "Bienvenido al sistema",
                };
                mail.IsBodyHtml = true;
                mail.Body = string.Format($@"
                                    <body>
                                    <h1 style=""background-color: #2196F3;
                                    font-size: x-large;
                                    font-family: 'Times New Roman', Times, serif;
                                        color: white; vertical-align: middle;
                                        margin: 0;"">Esto es un correo
                                    </h1>
                                    <p style=""font-family: 'Times New Roman', Times, serif;"">{mensaje}
                                    </p>
                                    <p>Este mensaje ha sido enviado el dia <b>{0:dd/MM/yyyy}</b> a las <b>{0:HH:mm:ss}</b> Hrs.
                                    </p>
                                </body>", DateTime.Now);
                mail.Bcc.Add("201g0264@rcarbonifera.tecnm.mx");
                SmtpClient client = new SmtpClient("smtp.outlook.office365.com");
                client.Port = 587;
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential cred = new(Usuario.Correo, Usuario.Contrasena);
                client.Credentials = cred;
                client.Send(mail);
                MessageBox.Show("El correo se ha enviado");
            }
            catch (Exception ex)
            {
                Error = $"Ha ocurrido un error: " + ex.Message;
            }
            Actualizar();

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Actualizar(string? prop = null)
        {
            TotalEmpleados = ListaUsuarios.Count;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
