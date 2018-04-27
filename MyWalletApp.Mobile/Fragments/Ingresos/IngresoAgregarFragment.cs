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
using System.Threading.Tasks;

namespace MyWalletApp.Mobile.Fragments.Ingresos
{
    public class IngresoAgregarFragment : Fragment
    {
        private EditText _monto;
        private EditText _descripcion;
        private Fuente fuente;
        private Spinner _fuenteDropDown;
        private Button _btnFechaIngreso;
        private TextView _fechaIngreso;
        private Button _btnGuardar;
        private IngresoService ingresoService;
        private FuenteService fuenteService;

        public IngresoAgregarFragment()
        {
            ingresoService = new IngresoService();
            fuenteService = new FuenteService();
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
            LoadFuentes();
            HandleEvents();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.IngresosAgregarFragment, container, false);
        }

        private async void LoadFuentes()
        {
            var array = await fuenteService.ObtenerFuentes();

            var adapter = new ArrayAdapter<Fuente>(this.Activity, Android.Resource.Layout.SimpleSpinnerItem, array.ToArray());
            _fuenteDropDown.Adapter = adapter;
        }

        private void FindViews()
        {
            _monto = View.FindViewById<EditText>(Resource.Id.txtMonto);
            _descripcion = View.FindViewById<EditText>(Resource.Id.txtDescripcion);
            _fuenteDropDown = View.FindViewById<Spinner>(Resource.Id.fuenteDropDown);
            _btnFechaIngreso = View.FindViewById<Button>(Resource.Id.btnFechaIngreso);
            _fechaIngreso = View.FindViewById<TextView>(Resource.Id.txtFechaIngreso);
            _btnGuardar = View.FindViewById<Button>(Resource.Id.btnGuardar);
        }

        private void HandleEvents()
        {
            _btnFechaIngreso.Click += _btnFechaIngreso_Click;
            _btnGuardar.Click += _btnGuardar_Click;
            _fuenteDropDown.ItemSelected += _fuenteDropDown_ItemSelected;
        }

        private void _fuenteDropDown_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            fuente = ((ArrayAdapter<Fuente>)spinner.Adapter).GetItem(e.Position);
        }

        private async void _btnGuardar_Click(object sender, EventArgs e)
        {
            if (!CamposInvalidos())
            {
                try
                {
                    var ingreso = new Ingreso()
                    {
                        Monto = Convert.ToDouble(_monto.Text),
                        Descripcion = _descripcion.Text,
                        FuenteId = fuente.Id,
                        Fuente = fuente,
                        Fecha = Convert.ToDateTime(_fechaIngreso.Text)
                    };

                    await ingresoService.AgregarIngreso(ingreso);
                    Toast.MakeText(this.Activity, "Ingreso agregado correctamente.", ToastLength.Long).Show();
                }
                catch(Exception ex)
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
                   fuente == null ||
                   _fechaIngreso.Text.Equals(string.Empty);
        }

        private void _btnFechaIngreso_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _fechaIngreso.Text = time.ToLongDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void LimpiarCampos()
        {
            _monto.Text = string.Empty;
            _descripcion.Text = string.Empty;
            fuente = null;
            _fechaIngreso.Text = string.Empty;
        }
    }
}