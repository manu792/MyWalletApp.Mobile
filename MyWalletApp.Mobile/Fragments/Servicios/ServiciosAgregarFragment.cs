using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MyWalletApp.Mobile.Models;
using MyWalletApp.Mobile.Services;
using MyWalletApp.Mobile.Helpers;

namespace MyWalletApp.Mobile.Fragments
{
    public class ServiciosAgregarFragment : Fragment
    {
        private EditText _nombre;
        private EditText _monto;
        private Spinner _tipoPago;
        private string tipoPago;
        private Button _btnFechaPago;
        private TextView _fechaPago;
        private Button _btnGuardar;
        private ServicioService servicioService;

        public ServiciosAgregarFragment()
        {
            servicioService = new ServicioService();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            FindViews();
            LoadTipoPagos();
            HandleEvents();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.ServiciosAgregarFragment, container, false);
        }

        private void LoadTipoPagos()
        {
            var array = TipoPago.TiposPago;

            var adapter = new ArrayAdapter<string>(this.Activity, Android.Resource.Layout.SimpleSpinnerItem, array);
            _tipoPago.Adapter = adapter;
        }

        private void FindViews()
        {
            _nombre = View.FindViewById<EditText>(Resource.Id.txtNombre);
            _monto = View.FindViewById<EditText>(Resource.Id.txtMonto);
            _tipoPago = View.FindViewById<Spinner>(Resource.Id.tipoPagoDropDown);
            _btnFechaPago = View.FindViewById<Button>(Resource.Id.btnFechaPago);
            _fechaPago = View.FindViewById<TextView>(Resource.Id.txtFechaPago);
            _btnGuardar = View.FindViewById<Button>(Resource.Id.btnGuardar);
        }

        private void HandleEvents()
        {
            _btnFechaPago.Click += _btnFechaPago_Click;
            _btnGuardar.Click += _btnGuardar_Click;
            _tipoPago.ItemSelected += _tipoPago_ItemSelected;
        }

        private void _tipoPago_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            tipoPago = ((ArrayAdapter<string>)spinner.Adapter).GetItem(e.Position);
        }

        private async void _btnGuardar_Click(object sender, EventArgs e)
        {
            if (!CamposInvalidos())
            {
                var servicio = new Servicio()
                {
                    Nombre = _nombre.Text,
                    Monto = Convert.ToDouble(_monto.Text),
                    EsPorMes = _tipoPago.SelectedItem.ToString().ToLower().Equals("mensualmente") ? true : false,
                    FechaPago = Convert.ToDateTime(_fechaPago.Text)
                };

                await servicioService.AgregarServicio(servicio);
            }
        }

        private bool CamposInvalidos()
        {
            return _nombre.Text.Equals(string.Empty) || 
                   _monto.Text.Equals(string.Empty) || 
                   _fechaPago.Text.Equals(string.Empty);
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