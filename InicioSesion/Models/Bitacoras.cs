using System;
using System.Collections.Generic;

namespace InicioSesion.Models;

public partial class Bitacoras
{
    public int Id { get; set; }

    public string Correo { get; set; } = null!;

    public string Observaciones { get; set; } = null!;

    public DateTime? Fecha { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Usuarios? IdUsuarioNavigation { get; set; }
}
