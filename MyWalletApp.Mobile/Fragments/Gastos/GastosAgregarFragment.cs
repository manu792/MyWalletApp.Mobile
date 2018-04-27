using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MyWalletApp.Mobile.Services;
using MyWalletApp.Mobile.Models;

namespace MyWalletApp.Mobile.Fragments.Gastos
{
    public class GastosAgregarFragment : Fragment
    {
        private EditText _monto;
        private EditText _descripcion;
        private Servicio servicio;
        private Spinner _servicioDropDown;
        private Button _btnFechaGasto;
        private TextView _fechaGasto;
        private Button _btnGuardar;
        private ServicioService servicioService;
        private GastoService gastoService;

        public GastosAgregarFragment()
        {
            servicioService = new ServicioService();
            gastoService = new GastoService();
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
            LoadServicios();
            HandleEvents();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.GastosAgregarFragment, container, false);
        }

        private async void LoadServicios()
        {
            var array = await servicioService.ObtenerServicios();

            var adapter = new ArrayAdapter<Servicio>(this.Activity, Android.Resource.Layout.SimpleSpinnerItem, array.ToArray());
            _servicioDropDown.Adapter = adapter;
        }

        private void FindViews()
        {
            _monto = View.FindViewById<EditText>(Resource.Id.txtMonto);
            _descripcion = View.FindViewById<EditText>(Resource.Id.txtDescripcion);
            _servicioDropDown = View.FindViewById<Spinner>(Resource.Id.servicioDropDown);
            _btnFechaGasto = View.FindViewById<Button>(Resource.Id.btnFechaGasto);
            _fechaGasto = View.FindViewById<TextView>(Resource.Id.txtFechaGasto);
            _btnGuardar = View.FindViewById<Button>(Resource.Id.btnGuardar);
        }

        private void HandleEvents()
        {
            _btnFechaGasto.Click += _btnFechaGasto_Click; ;
            _btnGuardar.Click += _btnGuardar_Click;
            _servicioDropDown.ItemSelected += _servicioDropDown_ItemSelected;
        }

        private void _servicioDropDown_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            servicio = ((ArrayAdapter<Servicio>)spinner.Adapter).GetItem(e.Position);
        }

        private void _btnFechaGasto_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance((time) =>
            {
                _fechaGasto.Text = time.ToLongDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private async void _btnGuardar_Click(object sender, EventArgs e)
        {
            if (!CamposInvalidos())
            {
                try
                {
                    var ingreso = new Gasto()
                    {
                        Monto = Convert.ToDouble(_monto.Text),
                        Descripcion = _descripcion.Text,
                        ServicioId = servicio.Id,
                        Servicio = servicio,
                        Fecha = Convert.ToDateTime(_fechaGasto.Text)
                    };

                    await gastoService.AgregarGasto(ingreso);
                    Toast.MakeText(this.Activity, "Gasto agregado correctamente.", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this.Activity, ex.Message, ToastLength.Long).Show();
                }
                finally
                {
                    LimpiarCampos();
                }
            }
        }

        private bool CamposInvalidos()
        {
            return _monto.Text.Equals(string.Empty) ||
                   _descripcion.Text.Equals(string.Empty) ||
                   servicio == null ||
                   _fechaGasto.Text.Equals(string.Empty);
        }

        private void LimpiarCampos()
        {
            _monto.Text = string.Empty;
            _descripcion.Text = string.Empty;
            servicio = null;
            _fechaGasto.Text = string.Empty;
        }
    }
}