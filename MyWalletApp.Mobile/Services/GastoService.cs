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
    public class GastoService
    {
        private BaseRepository<Gasto> gastoRepo;
        private const string RESOURCE_NAME = "gasto";

        public GastoService()
        {
            gastoRepo = new BaseRepository<Gasto>();
        }

        public async Task<IEnumerable<Gasto>> ObtenerGastos()
        {
            var gastos = await gastoRepo.GetAll(RESOURCE_NAME);
            return gastos.Where(s => s.Fecha != null);
        }

        public async Task AgregarGasto(Gasto gasto)
        {
            await gastoRepo.Agregar(gasto, RESOURCE_NAME);
        }

        public async Task ActualizarGasto(int id, Gasto gasto)
        {
            await gastoRepo.Update(id, gasto, RESOURCE_NAME);
        }

        public async Task ActualizarGasto(string id, Gasto gasto)
        {
            await gastoRepo.Update(id, gasto, RESOURCE_NAME);
        }

        public async Task EliminarGasto(int id)
        {
            await gastoRepo.Delete(id, RESOURCE_NAME);
        }

        public async Task EliminarGasto(string id)
        {
            await gastoRepo.Delete(id, RESOURCE_NAME);
        }
    }
}