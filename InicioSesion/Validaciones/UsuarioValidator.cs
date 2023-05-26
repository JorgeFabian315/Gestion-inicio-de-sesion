using FluentValidation;
using InicioSesion.Catalogos;
using InicioSesion.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InicioSesion.Validaciones
{
    public class UsuarioValidator : AbstractValidator<Usuarios>
    {
        UsuariosCatalogo catalogo = new();
        string _cadenanombre = @"\A[A-Za-zñÑ]{3,}(\s[A-Za-zñÑ]{3,}){0,3}\Z";
        Regex validarnombre;
        public UsuarioValidator(ObservableCollection<Usuarios>? lista = null)
        {
            validarnombre = new Regex(_cadenanombre);

            RuleSet("Correo", () =>
            {
                RuleFor(usuario => usuario.Correo)
                                                         .Cascade(CascadeMode.Stop)
                                                         .NotEmpty()
                                                         .WithMessage("El correo electrónico no puede estar vaciío")
                                                         .EmailAddress()
                                                         .WithMessage("El correo no cumple con el formato correcto")
                                                         .MaximumLength(90);
              
            });

            RuleSet("Contraseña", () =>
            {
                RuleFor(us => us.Contrasena)
                                                   .Cascade(CascadeMode.Stop)
                                                   .NotEmpty()
                                                   .WithMessage("La contraseña no puede estar vacía")
                                                   .MinimumLength(3)
                                                   .MaximumLength(100);
            });

            RuleFor(usuario => usuario.Nombre)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MaximumLength(50)
                .Must(us => validarnombre.IsMatch(us)).WithMessage("El apartartado {PropertyName} no cumple con el formato correcto");

            if(lista != null)
                RuleFor(us => us)
                                        .Must(us => !lista.Any(x => x.Correo == us.Correo && x.Id != us.Id))
                                        .WithMessage("El correo electrónico ya existe");


        }
    }
}
