﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGastos.Servicio.DTOs
{
   public class RegistrarUsuarioDto
   {
      public string Nombre { get; set; }
      public string Correo { get; set; }
      public string Contrasenia { get; set; }
   }
}
