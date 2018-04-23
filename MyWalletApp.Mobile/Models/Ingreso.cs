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
    public class Ingreso : IEntities
    {
        public int Id { get; set; }
        public double Monto { get; set; }
        public string Descripcion { get; set; }
        public int FuenteId { get; set; }
        public DateTime Fecha { get; set; }

        // Navigation property
        public virtual Fuente Fuente { get; set; }
    }
}