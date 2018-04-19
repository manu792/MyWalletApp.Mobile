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

namespace MyWalletApp.Mobile.Adapters
{
    public class ServicioListAdapter : BaseAdapter<Servicio>
    {
        IList<Servicio> items;
        Activity context;

        public ServicioListAdapter(Activity context, IList<Servicio> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Servicio this[int position] => items[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            if (convertView == null)
                convertView = context.LayoutInflater.Inflate(Resource.Layout.ServicioRowView, null);

            convertView.FindViewById<TextView>(Resource.Id.nombre).Text = item.Nombre;
            convertView.FindViewById<TextView>(Resource.Id.monto).Text = item.Monto.ToString();
            convertView.FindViewById<TextView>(Resource.Id.fechaPago).Text = item.FechaPago.ToString();

            return convertView;
        }
    }
}