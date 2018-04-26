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
using MyWalletApp.Mobile.Services;
using MyWalletApp.Mobile.Models;
using MyWalletApp.Mobile.Adapters;

namespace MyWalletApp.Mobile
{
    [Activity(Label = "Notificaciones")]
    public class NotificacionActivity : Activity
    {
        private ListView _listView;
        private ServicioService _servicioService;
        private IList<Servicio> _servicios; 

        public NotificacionActivity()
        {
            _servicioService = new ServicioService();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Notificaciones);
            Init();
            LoadNotificaciones();
        }

        private void Init()
        {
            _listView = FindViewById<ListView>(Resource.Id.notificacionesListView);
        }

        private async void LoadNotificaciones()
        {
            _servicios = (await _servicioService.ServiciosAPagarEnProximosCincoDias()).ToList();
            _listView.Adapter = new ServicioListAdapter(this, _servicios);
            ConfigurarAlturaListView();
        }

        private void ConfigurarAlturaListView()
        {
            var listAdapter = _listView.Adapter;
            if (listAdapter == null)
                return;

            int desiredWidth = View.MeasureSpec.MakeMeasureSpec(_listView.Width, MeasureSpecMode.Unspecified);
            int totalHeight = 0;
            View view = null;
            for (int i = 0; i < listAdapter.Count; i++)
            {
                view = listAdapter.GetView(i, view, _listView);
                if (i == 0)
                    view.LayoutParameters = (new ViewGroup.LayoutParams(desiredWidth, WindowManagerLayoutParams.WrapContent));

                view.Measure(desiredWidth, (int)MeasureSpecMode.Unspecified);
                totalHeight += view.MeasuredHeight;
            }
            ViewGroup.LayoutParams prm = _listView.LayoutParameters;
            prm.Height = totalHeight + (_listView.DividerHeight * (listAdapter.Count - 1));
            _listView.LayoutParameters = prm;
        }
    }
}