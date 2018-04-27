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
using MyWalletApp.Mobile.Adapters;

namespace MyWalletApp.Mobile.Fragments.Gastos
{
    public class GastosBuscarFragment : Fragment
    {
        private EditText _buscar;
        private ListView _listView;
        private EditText _monto;
        private EditText _descripcion;
        private Spinner _servicio;
        private Servicio _servicioSeleccionado;
        private Button _btnFechaGasto;
        private TextView _fechaGasto;
        private Button _btnActualizar;
        private Button _btnEliminar;
        private ServicioService _servicioService;
        private GastoService _gastoService;
        private IEnumerable<Gasto> _gastos;
        private IList<Gasto> _filteredList;
        private Gasto _gastoSeleccionado;
        private IList<Servicio> _servicios;

        public GastosBuscarFragment()
        {
            _servicioService = new ServicioService();
            _gastoService = new GastoService();
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
            ActualizarGastos();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.GastosBuscarFragment, container, false);
        }

        private async void ActualizarGastos()
        {
            _gastos = await _gastoService.ObtenerGastos();
            _filteredList = _gastos.ToList();
            _gastoSeleccionado = null;
            _listView.Adapter = new GastoListAdapter(this.Activity, _filteredList);

            ConfigurarAlturaListView();

            LimpiarCampos();
        }

        private async void LoadFuentes()
        {
            _servicios = (await _servicioService.ObtenerServicios()).ToList();

            var adapter = new ArrayAdapter<Servicio>(this.Activity, Android.Resource.Layout.SimpleSpinnerItem, _servicios.ToArray());
            _servicio.Adapter = adapter;
        }

        private void FindViews()
        {
            _buscar = View.FindViewById<EditText>(Resource.Id.buscar);
            _listView = View.FindViewById<ListView>(Resource.Id.gastosListView);
            _monto = View.FindViewById<EditText>(Resource.Id.txtMonto);
            _descripcion = View.FindViewById<EditText>(Resource.Id.txtDescripcion);
            _servicio = View.FindViewById<Spinner>(Resource.Id.servicioDropDown);
            _btnFechaGasto = View.FindViewById<Button>(Resource.Id.btnFechaGasto);
            _fechaGasto = View.FindViewById<TextView>(Resource.Id.txtFechaGasto);
            _btnActualizar = View.FindViewById<Button>(Resource.Id.btnActualizar);
            _btnEliminar = View.FindViewById<Button>(Resource.Id.btnEliminar);
        }

        private void HandleEvents()
        {
            _buscar.TextChanged += _buscar_TextChanged;
            _listView.ItemClick += _listView_ItemClick;
            _servicio.ItemSelected += _servicio_ItemSelected;
            _btnFechaGasto.Click += _btnFechaGasto_Click;
            _btnActualizar.Click += _btnActualizar_Click;
            _btnEliminar.Click += _btnEliminar_Click;
        }

        private void _btnFechaGasto_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance((time) =>
            {
                _fechaGasto.Text = time.ToLongDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void _servicio_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            _servicioSeleccionado = ((ArrayAdapter<Servicio>)spinner.Adapter).GetItem(e.Position);
        }

        private void _buscar_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            var searchTerm = _buscar.Text;

            _filteredList = _gastos.Where(c => c.Descripcion.Contains(searchTerm) ||
                c.Monto.ToString().Contains(searchTerm) ||
                c.Servicio.Nombre.Contains(searchTerm) ||
                c.Fecha.ToString().Contains(searchTerm)).ToList();

            var filteredAdapter = new GastoListAdapter(this.Activity, _filteredList);
            _listView.Adapter = filteredAdapter;
        }

        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            _gastoSeleccionado = _filteredList[e.Position];

            _monto.Text = _gastoSeleccionado.Monto.ToString();
            _descripcion.Text = _gastoSeleccionado.Descripcion;
            
            _servicio.SetSelection(GetIndex());
            _fechaGasto.Text = _gastoSeleccionado.Fecha.ToString();
        }

        private int GetIndex()
        {
            for (int i = 0; i < _servicios.Count; i++)
                if (_servicios[i].Id == _gastoSeleccionado.Servicio.Id)
                    return i;

            return 0;
        }

        private async void _btnEliminar_Click(object sender, EventArgs e)
        {
            if (_gastoSeleccionado != null)
            {
                try
                {
                    await _gastoService.EliminarGasto(_gastoSeleccionado.Id);
                    Toast.MakeText(this.Activity, "Se ha eliminado el ingreso correctamente.", ToastLength.Long)
                        .Show();
                    LimpiarCampos();
                }
                catch
                {
                    Toast.MakeText(this.Activity, "Hubo un problema al tratar de eliminar el ingreso. Intente de nuevo mas tarde.",
                       ToastLength.Long).Show();
                }

                ActualizarGastos();
            }
        }

        private async void _btnActualizar_Click(object sender, EventArgs e)
        {
            if (_gastoSeleccionado != null)
            {
                try
                {
                    var gasto = new Gasto()
                    {
                        Id = _gastoSeleccionado.Id,
                        Descripcion = _descripcion.Text,
                        Monto = Convert.ToDouble(_monto.Text),
                        Fecha = Convert.ToDateTime(_fechaGasto.Text),
                        Servicio = _servicioSeleccionado,
                        ServicioId = _servicioSeleccionado.Id
                    };
                    await _gastoService.ActualizarGasto(_gastoSeleccionado.Id, gasto);
                    Toast.MakeText(this.Activity, "Se ha actualizado el servicio correctamente.", ToastLength.Long)
                        .Show();
                    LimpiarCampos();
                }
                catch
                {
                    Toast.MakeText(this.Activity, "Hubo un problema al tratar de actualizar el servicio. Intente de nuevo mas tarde.",
                       ToastLength.Long).Show();
                }

                ActualizarGastos();
            }
        }

        private void LimpiarCampos()
        {
            _listView.SetSelection(0);
            _monto.Text = string.Empty;
            _descripcion.Text = string.Empty;
            _fechaGasto.Text = string.Empty;
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