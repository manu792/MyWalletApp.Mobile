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

namespace MyWalletApp.Mobile.Fragments
{
    public class ServiciosBuscarFragment : Fragment
    {
        private EditText _nombre;
        private EditText _monto;
        private EditText _tipoPago;
        private Button _btnFechaPago;
        private TextView _fechaPago;
        private Button _btnGuardar;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            FindViews();
            HandleEvents();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.ServiciosAgregarFragment, container, false);
        }

        private void FindViews()
        {
            _nombre = View.FindViewById<EditText>(Resource.Id.txtNombre);
            _monto = View.FindViewById<EditText>(Resource.Id.txtMonto);
            _tipoPago = View.FindViewById<EditText>(Resource.Id.tipoPagoDropDown);
            _btnFechaPago = View.FindViewById<Button>(Resource.Id.btnFechaPago);
            _fechaPago = View.FindViewById<TextView>(Resource.Id.txtFechaPago);
            _btnGuardar = View.FindViewById<Button>(Resource.Id.btnGuardar);
        }

        private void HandleEvents()
        {
            _btnFechaPago.Click += _btnFechaPago_Click;
            _btnGuardar.Click += _btnGuardar_Click;
        }

        private void _btnGuardar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _btnFechaPago_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _fechaPago.Text = time.ToLongDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }
    }
}