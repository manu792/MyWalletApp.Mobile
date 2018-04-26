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
    public class FuenteService
    {
        private BaseRepository<Fuente> ingresoRepo;
        private const string RESOURCE_NAME = "fuente";

        public FuenteService()
        {
            ingresoRepo = new BaseRepository<Fuente>();
        }

        public async Task<IEnumerable<Fuente>> ObtenerFuentes()
        {
            var ingresos = await ingresoRepo.GetAll(RESOURCE_NAME);
            return ingresos;
        }

        public async Task AgregarFuente(Fuente fuente)
        {
            await ingresoRepo.Agregar(fuente, RESOURCE_NAME);
        }

        public async Task ActualizarFuente(int id, Fuente fuente)
        {
            await ingresoRepo.Update(id, fuente, RESOURCE_NAME);
        }

        public async Task ActualizarFuente(string id, Fuente fuente)
        {
            await ingresoRepo.Update(id, fuente, RESOURCE_NAME);
        }

        public async Task EliminarFuente(int id)
        {
            await ingresoRepo.Delete(id, RESOURCE_NAME);
        }

        public async Task EliminarFuente(string id)
        {
            await ingresoRepo.Delete(id, RESOURCE_NAME);
        }
    }
}