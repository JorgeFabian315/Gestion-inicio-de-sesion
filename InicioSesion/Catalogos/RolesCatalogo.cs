using InicioSesion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InicioSesion.Catalogos
{
    public class RolesCatalogo
    {

        InicioSesionContext context = new();

        public IEnumerable<Roles> GetRoles()
        {
            return context.Roles.OrderBy(rol=> rol.Nombre);
        }
        public Roles? GetRol(int id)
        {
            var rol = context.Roles.Find(id);
            if (rol != null)
                return rol;
            else
                return null;
        }

    }
}
