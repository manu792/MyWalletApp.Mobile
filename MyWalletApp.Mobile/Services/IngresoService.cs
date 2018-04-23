using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyWalletApp.Mobile.Data.Repositories;
using MyWalletApp.Mobile.Models;
using System.Threading.Tasks;

namespace MyWalletApp.Mobile.Services
{
    public class IngresoService
    {
        private BaseRepository<Ingreso> ingresoRepo;
        private const string RESOURCE_NAME = "ingreso";

        public IngresoService()
        {
            ingresoRepo = new BaseRepository<Ingreso>();
        }

        public async Task<IEnumerable<Ingreso>> ObtenerIngresos()
        {
            var ingresos = await ingresoRepo.GetAll(RESOURCE_NAME);
            return ingresos;
        }

        public async Task AgregarIngreso(Ingreso ingreso)
        {
            await ingresoRepo.Agregar(ingreso, RESOURCE_NAME);
        }

        public async Task ActualizarIngreso(int id, Ingreso ingreso)
        {
            await ingresoRepo.Update(id, ingreso, RESOURCE_NAME);
        }

        public async Task ActualizarIngresp(string id, Ingreso ingreso)
        {
            await ingresoRepo.Update(id, ingreso, RESOURCE_NAME);
        }

        public async Task EliminarIngreso(int id)
        {
            await ingresoRepo.Delete(id, RESOURCE_NAME);
        }

        public async Task EliminarIngreso(string id)
        {
            await ingresoRepo.Delete(id, RESOURCE_NAME);
        }
    }
}