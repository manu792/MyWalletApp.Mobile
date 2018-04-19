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
using MyWalletApp.Mobile.Models;
using MyWalletApp.Mobile.Data.Repositories;
using System.Threading.Tasks;

namespace MyWalletApp.Mobile.Services
{
    public class ServicioService
    {
        private BaseRepository<Servicio> servicioRepo;
        private const string RESOURCE_NAME = "servicio";

        public ServicioService()
        {
            servicioRepo = new BaseRepository<Servicio>();
        }

        public async Task<IEnumerable<Servicio>> ObtenerServicios()
        {
            var servicios = await servicioRepo.GetAll(RESOURCE_NAME);
            return servicios.Where(s => s.FechaPago != null);
        }

        public async Task AgregarServicio(Servicio servicio)
        {
            await servicioRepo.Agregar(servicio, RESOURCE_NAME);
        }

        public async Task ActualizarServicio(int id, Servicio servicio)
        {
            await servicioRepo.Update(id, servicio, RESOURCE_NAME);
        }

        public async Task ActualizarServicio(string id, Servicio servicio)
        {
            await servicioRepo.Update(id, servicio, RESOURCE_NAME);
        }

        public async Task EliminarServicio(int id)
        {
            await servicioRepo.Delete(id, RESOURCE_NAME);
        }

        public async Task EliminarServicio(string id)
        {
            await servicioRepo.Delete(id, RESOURCE_NAME);
        }
    }
}