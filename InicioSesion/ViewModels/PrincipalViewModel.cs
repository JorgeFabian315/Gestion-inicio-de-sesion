using GalaSoft.MvvmLight.Command;
using InicioSesion.Catalogos;
using InicioSesion.Models;
using InicioSesion.Validaciones;
using InicioSesion.Views;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using FluentValidation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace InicioSesion.ViewModels
{
    public class PrincipalViewModel : INotifyPropertyChanged
    {
        public Usuarios? Usuario { get; set; }
        public bool EstaConectado => Usuario.Id != 0;

        public ObservableCollection<Roles> ListaRoles { get; set; } = new();

        UsuariosCatalogo cusuario = new();
        RolesCatalogo crol = new();

        public string Error { get; set; } = "";
        public UserControl Vista { get; set; }

        public Contraseña? Contraseña { get; set; }

        LoginView loginview;

        UsuarioValidator validator;
        public ICommand IniciarSesionCommand { get; set; }
        public ICommand CerrarSesionCommand { get; set; }
        public ICommand CrearCuentaCommand { get; set; }

        public ICommand CrearNuevoUsuarioCommand => new RelayCommand<int>(CrearNuevoUsuario);
        public ICommand VerCambiarContraseñaCommand => new RelayCommand(VerCambiarContraseña);
        public ICommand CambiarContraseñaCommand => new RelayCommand(CambiarContraseña);

        private void VerCambiarContraseña()
        {
            if (Usuario != null)
            {
                Contraseña = new();
                Vista = new CambiarContraseñaView();
                Actualizar();
            }
        }

        public void CambiarContraseña()
        {
            if(Usuario != null && Contraseña is not null)
            {
                Error = "";
                if (Contraseña.NuevaContraseña == Contraseña.ConfirmarContraseña)
                {
                    cusuario.CambiarContraseña(Usuario.Correo, Contraseña.NuevaContraseña);
                    MessageBox.Show("La contraseña se cambio con exito");
                    CerrarSesion();
                }
                else
                    Error = "La contraseña no es igaual a la confirmación";

                Actualizar();
            }
        }

        private void CrearNuevoUsuario(int id)
        {
            if (Usuario != null)
            {
                validator = new();
                var result = validator.Validate(Usuario,
                    option => option.IncludeAllRuleSets());
                Error = "";
                if (result.IsValid)
                {
                    Usuario.IdRol = id;
                    cusuario.Agregar(Usuario);
                    Vista = new UsuariosView();
                }
                else
                {
                    foreach (var error in result.Errors)
                        Error = $"{Error} {error} {Environment.NewLine}";
                }

                Actualizar();
            }
        }

        public PrincipalViewModel()
        {
            Usuario = new();
            IniciarSesionCommand = new RelayCommand(IniciarSesion);
            CerrarSesionCommand = new RelayCommand(CerrarSesion);
            CrearCuentaCommand = new RelayCommand(CrearCuenta);
            loginview = new()
            {
                DataContext = this
            };
            Vista = loginview;
            GetRoles();
        }
        private void CrearCuenta()
        {
            Usuario = new();
            Error = "";
            Vista = new AgregarUsuarioView()
            {
                DataContext = this
            };
            Actualizar();
        }
        private void CerrarSesion()
        {
            Usuario = new();
            Vista = loginview;
            Error = "";
            Actualizar();
        }

        private void IniciarSesion()
        {
            if (Usuario is not null)
            {

                Error = "";
                validator = new();
                var result = validator.Validate(Usuario, option=> option.IncludeRuleSets("Contraseña", "Correo"));

                if (result.IsValid)
                {
                    var inicio =  cusuario.spIniciarSesion(Usuario.Correo, Usuario.Contrasena);

                    if (inicio == 1)
                    {
                        var usconectado = cusuario.GetUsuario(Usuario.Correo);
                        Usuario = usconectado;

                        if (Thread.CurrentPrincipal is not null)
                        {
                            // var nose = Thread.CurrentPrincipal.Identity.Name;
                            if (Thread.CurrentPrincipal.IsInRole("Administrador"))
                                AccionesAdministrador();
                            if (Thread.CurrentPrincipal.IsInRole("Capturista"))
                                AccionesUsuarioCapturista();
                        }
                    }
                    else
                        Error = inicio == 2 ? "Usuario no encontrado" : "Contraseña incorrecta";
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        Error = $"{Error} {item} {Environment.NewLine}";
                    }
                }

                Actualizar();

            }
        }

        [Authorize(Roles = "Administrador")] // No es necesario
        private void AccionesUsuarioCapturista()
        {
            Vista = new UsuariosView() { };
        }

        public void GetRoles()
        {
            ListaRoles.Clear();
            var listarol = crol.GetRoles();
            foreach (var item in listarol)
            {
                ListaRoles.Add(item);
            }
            Actualizar();
        }

        private void AccionesAdministrador()
        {
            Vista = new BienvenidoView();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Actualizar(string? prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
