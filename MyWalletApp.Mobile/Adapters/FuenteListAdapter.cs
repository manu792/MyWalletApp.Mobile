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
    public class FuenteListAdapter : BaseAdapter<Fuente>
    {
        IList<Fuente> items;
        Activity context;

        public FuenteListAdapter(Activity context, IList<Fuente> items) : base()
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

        public override Fuente this[int position] => items[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            if (convertView == null)
                convertView = context.LayoutInflater.Inflate(Resource.Layout.FuenteRowView, null);

            convertView.FindViewById<TextView>(Resource.Id.nombre).Text = item.Nombre;

            return convertView;
        }
    }
}