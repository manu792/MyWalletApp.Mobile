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

namespace MyWalletApp.Mobile.Models
{
    public class Servicio : IEntities
    {
        public string Nombre { get; set; }
        public double Monto { get; set; }
        public bool? EsPorMes { get; set; }
        public DateTime FechaPago { get; set; }
    }
}