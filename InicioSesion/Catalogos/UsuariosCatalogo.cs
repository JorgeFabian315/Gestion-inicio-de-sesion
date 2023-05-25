using InicioSesion.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Security.Principal;
using System.Threading.Tasks;

namespace InicioSesion.Catalogos
{
    public class UsuariosCatalogo
    {
        // scaffold-dbcontext "server=localhost; password=root; user=root; database=InicioSesion" Pomelo.EntityFrameworkCore.MySql -o "Models" -nopluralize -f
        InicioSesionContext context = new();
        public Usuarios? GetUsuario(string correo)
        {
            return context.Usuarios.Include(d => d.Bitacoras).FirstOrDefault(c => c.Correo == correo);
        }
        public int spIniciarSesion(string correo, string contraseña)
        {
            string comando = $"SELECT fnInicioSesion('{correo}','{contraseña}')";
            var y = ((IEnumerable<int>) context.Database.
                SqlQueryRaw<int>(comando, correo, contraseña).AsAsyncEnumerable<int>()).First();
            if(y == 1)
            {
                var us = context.Usuarios.Include(c => c.IdRolNavigation).FirstOrDefault(x => x.Correo == correo);
                if (us != null)
                    EstablecerTipoUusuario(us);

            }

            return y;
        }

        private void EstablecerTipoUusuario(Usuarios u)
        {
            // Crear una identidad con el nombre del usuario 
            GenericIdentity user = new GenericIdentity(u.Nombre);
            if (user is not null)
            {
                // Creamos los roles
                string[] roles = new string[]
                {
                    u.IdRolNavigation.Nombre
                };
                // Creamos la credencial 
                GenericPrincipal credencial_usuario = new GenericPrincipal(user,roles);
                // Asignamos la credencial a la aplicacion
                Thread.CurrentPrincipal = credencial_usuario;

            }

        }


        public bool ValidarInicio(Usuarios usuario, out List<string> lista) 
        {
            lista = new();
            string cadenacorreo = @"[A-Za-z0-9ñÑ]+\@[A-Za-z0-9ñÑ]+\.(com|edu|tec|net|org|.gob)(\.(mx|ar))?";
            Regex validarcorreo = new(cadenacorreo);

            if(usuario != null)
            {
                if (string.IsNullOrWhiteSpace(usuario.Correo))
                    lista.Add("El correo no puede estar vacío");
                else if(usuario.Correo.Length > 90)
                    lista.Add("El correo ha excedido el tamaño establecido");
                else if(!validarcorreo.IsMatch(usuario.Correo))
                    lista.Add("El correo no tiene el formato correcto");


                if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                    lista.Add("La contraseña no puede estar vacío");
                else if (usuario.Contrasena.Length > 256)
                    lista.Add("La contraseña ha excedido el tamaño establecido");
            }
            return lista.Count == 0;
        }

        public void CambiarContraseña(string correo, string nuevacontraseña)
        {
            context.Database.ExecuteSqlRaw($"CALL spEncriptarContraseña('{correo}','{nuevacontraseña}');");
            //context.Database.BeginTransaction();
            //context.Database.CommitTransaction();
            //context.Database.RollbackTransaction();
        }

        public bool ValidarUsuario(Usuarios usuario, out List<string> lista)
        {
            lista = new();
            string _cadenanombre = @"\A[A-Za-zñÑ]+(\s[A-Za-zñÑ]+){0,3}\Z";
            Regex validarnombre = new Regex(_cadenanombre);
            string _cadenacorreo = @"[A-Za-z0-9ñÑ]+\@[A-Za-z0-9ñÑ]+\.(com|edu|tecnm|net|org|.gob)(\.(mx|ar))?";
          //  string _validarcontraseña = @"[A-Za-zñÑ0-9#@:]+";
            Regex validarcorreo = new(_cadenacorreo);

            if (usuario != null)
            {
                if (string.IsNullOrWhiteSpace(usuario.Correo))
                    lista.Add("El correo no puede estar vacío");
                else if (usuario.Correo.Length > 90)
                    lista.Add("El correo ha excedido el tamaño establecido");
                else if (!validarcorreo.IsMatch(usuario.Correo))
                    lista.Add("El correo no tiene el formato correcto");
                else if(context.Usuarios.Any(s => s.Correo == usuario.Correo && s.Id != usuario.Id))
                    lista.Add("El correo ya existe");



                if (string.IsNullOrWhiteSpace(usuario.Contrasena))
                    lista.Add("La contraseña no puede estar vacío");
                else if (usuario.Contrasena.Length > 60)
                    lista.Add("La contraseña ha excedido el tamaño establecido (1-60)");
                else if (usuario.Contrasena.Length <= 5)
                    lista.Add("La contraseña es demasiado corta");



                if (string.IsNullOrWhiteSpace(usuario.Nombre))
                    lista.Add("El nombre no puede estar vacío");
                else if(!validarnombre.IsMatch(usuario.Nombre))
                    lista.Add("El nombre no tiene el formato correcto");
                else if (usuario.Nombre.Length > 100)
                    lista.Add("El nombre ha excedido el tamaño establecido");
                else if (usuario.Nombre.Length < 3)
                    lista.Add("El nombre es demasiado corto");

            }
            return lista.Count == 0;
        }
        public IEnumerable<Usuarios> GetUsuarios()
        {
            return context.Usuarios.OrderBy(c => c.Nombre);
        }
        public void Agregar(Usuarios u)
        {
            context.Add(u);
            context.SaveChanges();
        }
        public void Eliminar(Usuarios u)
        {
            context.Remove(u);
            //context.Usuarios.Where(c => c.Id == u.Id).ExecuteDelete();
            context.SaveChanges();
        }
        public void Editar(Usuarios u)
        {
             context.Update(u);
             context.SaveChanges();
        }

        public void Recargar(Usuarios u)
        {
            context.Entry(u).Reload();
        }


      



    }
}
