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
    public class IngresoListAdapter : BaseAdapter<Ingreso>
    {
        IList<Ingreso> items;
        Activity context;

        public IngresoListAdapter(Activity context, IList<Ingreso> items) : base()
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

        public override Ingreso this[int position] => items[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            if (convertView == null)
                convertView = context.LayoutInflater.Inflate(Resource.Layout.IngresoRowView, null);

            convertView.FindViewById<TextView>(Resource.Id.descripcion).Text = item.Descripcion;
            convertView.FindViewById<TextView>(Resource.Id.monto).Text = item.Monto.ToString();
            convertView.FindViewById<TextView>(Resource.Id.fuente).Text = item.Fuente.Nombre;
            convertView.FindViewById<TextView>(Resource.Id.fechaIngreso).Text = item.Fecha.ToString();

            return convertView;
        }
    }
}