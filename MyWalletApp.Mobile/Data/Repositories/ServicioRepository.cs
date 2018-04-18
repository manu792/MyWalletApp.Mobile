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

namespace MyWalletApp.Mobile.Data.Repositories
{
    public class ServicioRepository : BaseRepository<Servicio>
    {
        public void AgregarServicio(Servicio servicio)
        {
            base.Agregar(servicio, base.Url + "servicio");
        }
    }
}