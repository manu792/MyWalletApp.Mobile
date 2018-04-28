using Android.App;
using Android.Widget;
using Android.OS;
using MyWalletApp.Mobile.Services;
using System.Collections.Generic;
using MyWalletApp.Mobile.Models;

namespace MyWalletApp.Mobile
{
    [Activity(Label = "MyWalletApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Button ingresosBtn;
        private Button gastosBtn;
        private Button serviciosBtn;
        private Button fuentesBtn;
        private Button notificacionesBtn;
        private ServicioService servicioService;
        private IList<Servicio> notificaciones;

        public MainActivity()
        {
            servicioService = new ServicioService();
            notificaciones = new List<Servicio>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Init();
            SetEventHandlers();
        }

        protected override void OnStart()
        {
            base.OnStart();
            LoadNotificaciones();
        }

        private void Init()
        {
            ingresosBtn = FindViewById<Button>(Resource.Id.ingresosBtn);
            gastosBtn = FindViewById<Button>(Resource.Id.gastosBtn);
            serviciosBtn = FindViewById<Button>(Resource.Id.serviciosBtn);
            fuentesBtn = FindViewById<Button>(Resource.Id.fuentesBtn);
            notificacionesBtn = FindViewById<Button>(Resource.Id.notificacionesBtn);
        }

        private void SetEventHandlers()
        {
            ingresosBtn.Click += IngresosBtn_Click;
            gastosBtn.Click += GastosBtn_Click;
            serviciosBtn.Click += ServiciosBtn_Click;
            fuentesBtn.Click += FuentesBtn_Click;
            notificacionesBtn.Click += NotificacionesBtn_Click;
        }

        private async void LoadNotificaciones()
        {
            var servicios = await servicioService.ServiciosAPagarEnProximosCincoDias();
            notificaciones = (IList<Servicio>)servicios;
            notificacionesBtn.Text = $"{notificaciones.Count} Notificacion(es)";
        }

        private void NotificacionesBtn_Click(object sender, System.EventArgs e)
        {
            if (notificaciones.Count > 0)
            {
                StartActivity(typeof(NotificacionActivity));
            }
            else
                Toast.MakeText(this, "No hay notificaciones en este momento", ToastLength.Long).Show();
        }

        private void FuentesBtn_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(FuentesActivity));
        }

        private void ServiciosBtn_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(ServiciosActivity));
        }

        private void GastosBtn_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(GastosActivity));
        }

        private void IngresosBtn_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(IngresosActivity));
        }
    }
}

