using Android.App;
using Android.Widget;
using Android.OS;

namespace MyWalletApp.Mobile
{
    [Activity(Label = "MyWalletApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Button ingresosBtn;
        private Button gastosBtn;
        private Button serviciosBtn;
        private Button fuentesBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            Init();
            SetEventHandlers();
        }

        private void Init()
        {
            ingresosBtn = FindViewById<Button>(Resource.Id.ingresosBtn);
            gastosBtn = FindViewById<Button>(Resource.Id.gastosBtn);
            serviciosBtn = FindViewById<Button>(Resource.Id.serviciosBtn);
            fuentesBtn = FindViewById<Button>(Resource.Id.fuentesBtn);
        }

        private void SetEventHandlers()
        {
            ingresosBtn.Click += IngresosBtn_Click;
            gastosBtn.Click += GastosBtn_Click;
            serviciosBtn.Click += ServiciosBtn_Click;
            fuentesBtn.Click += FuentesBtn_Click;
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

