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

namespace MyWalletApp.Mobile.Services
{
    public class ServicioService
    {
        private ServicioRepository servicioRepo;

        public ServicioService()
        {
            servicioRepo = new ServicioRepository();
        }

        public void AgregarServicio(Servicio servicio)
        {
            servicioRepo.AgregarServicio(servicio);
        }
    }
}