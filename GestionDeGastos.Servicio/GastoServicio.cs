using GestionDeGastos.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeGastos.Servicio
{
    public interface IGastoServicio
    {
        void AgregarGastoAsync(Gasto gasto, string tipoDeGasto);
    }
    public class GastoServicio : IGastoServicio
    {
        public void AgregarGastoAsync(Gasto gasto, string tipoDeGasto)
        {
        }
    }
}
