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

namespace MyWalletApp.Mobile.Helpers
{
    public static class TipoPago
    {
        private static IList<string> _tiposPago = new List<string>()
        {
            "Mensualmente",
            "Anualmente"
        };

        public static IList<string> TiposPago
        {
            get
            {
                return _tiposPago;
            }
        }
    }
}