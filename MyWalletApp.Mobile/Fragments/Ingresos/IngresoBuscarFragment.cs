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
using MyWalletApp.Mobile.Models;
using MyWalletApp.Mobile.Services;
using MyWalletApp.Mobile.Adapters;

namespace MyWalletApp.Mobile.Fragments.Ingresos
{
    public class IngresoBuscarFragment : Fragment
    {
        private EditText _buscar;
        private ListView _listView;
        private EditText _monto;
        private EditText _descripcion;
        private Spinner _fuente;
        private Fuente _fuenteSeleccionada;
        private Button _btnFechaIngreso;
        private TextView _fechaIngreso;
        private Button _btnActualizar;
        private Button _btnEliminar;
        private FuenteService _fuenteService;
        private IngresoService _ingresoService;
        private IEnumerable<Ingreso> _ingresos;
        private IList<Ingreso> _filteredList;
        private Ingreso _ingresoSeleccionado;
        private ProgressBar _loadingSpinner;
        private IList<Fuente> _fuentes;

        public IngresoBuscarFragment()
        {
            _fuenteService = new FuenteService();
            _ingresoService = new IngresoService();
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
            ActualizarIngresos();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.IngresosBuscarFragment, container, false);
        }

        private async void ActualizarIngresos()
        {
            try
            {
                _loadingSpinner.Visibility = ViewStates.Visible;

                _ingresos = await _ingresoService.ObtenerIngresos();
                _filteredList = _ingresos.ToList();
                _ingresoSeleccionado = null;
                _listView.Adapter = new IngresoListAdapter(this.Activity, _filteredList);

                ConfigurarAlturaListView();

                LimpiarCampos();
            }
            catch
            {

            }
            finally
            {
                _loadingSpinner.Visibility = ViewStates.Gone;
            }
        }

        private async void LoadFuentes()
        {
            _fuentes = (await _fuenteService.ObtenerFuentes()).ToList();

            var adapter = new ArrayAdapter<Fuente>(this.Activity, Android.Resource.Layout.SimpleSpinnerItem, _fuentes.ToArray());
            _fuente.Adapter = adapter;
        }

        private void FindViews()
        {
            _buscar = View.FindViewById<EditText>(Resource.Id.buscar);
            _listView = View.FindViewById<ListView>(Resource.Id.ingresosListView);
            _monto = View.FindViewById<EditText>(Resource.Id.txtMonto);
            _descripcion = View.FindViewById<EditText>(Resource.Id.txtDescripcion);
            _fuente = View.FindViewById<Spinner>(Resource.Id.fuenteDropDown);
            _btnFechaIngreso = View.FindViewById<Button>(Resource.Id.btnFechaIngreso);
            _fechaIngreso = View.FindViewById<TextView>(Resource.Id.txtFechaIngreso);
            _btnActualizar = View.FindViewById<Button>(Resource.Id.btnActualizar);
            _btnEliminar = View.FindViewById<Button>(Resource.Id.btnEliminar);
            _loadingSpinner = View.FindViewById<ProgressBar>(Resource.Id.loadingSpinner);
        }

        private void HandleEvents()
        {
            _buscar.TextChanged += _buscar_TextChanged;
            _listView.ItemClick += _listView_ItemClick;
            _fuente.ItemSelected += _fuente_ItemSelected;
            _btnFechaIngreso.Click += _btnFechaIngreso_Click;
            _btnActualizar.Click += _btnActualizar_Click;
            _btnEliminar.Click += _btnEliminar_Click;
        }

        private void _buscar_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            var searchTerm = _buscar.Text;

            _filteredList = _ingresos.Where(c => c.Descripcion.Contains(searchTerm) || 
                c.Monto.ToString().Contains(searchTerm) || 
                c.Fuente.Nombre.Contains(searchTerm) ||
                c.Fecha.ToString().Contains(searchTerm)).ToList();

            var filteredAdapter = new IngresoListAdapter(this.Activity, _filteredList);
            _listView.Adapter = filteredAdapter;
        }

        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            _ingresoSeleccionado = _filteredList[e.Position];

            _monto.Text = _ingresoSeleccionado.Monto.ToString();
            _descripcion.Text = _ingresoSeleccionado.Descripcion;
            _fuente.SetSelection(GetIndex());
            _fechaIngreso.Text = _ingresoSeleccionado.Fecha.ToString();
        }

        private int GetIndex()
        {
            for (int i = 0; i < _fuentes.Count; i++)
                if (_fuentes[i].Id == _ingresoSeleccionado.Fuente.Id)
                    return i;

            return 0;
        }

        private async void _btnEliminar_Click(object sender, EventArgs e)
        {
            if (_ingresoSeleccionado != null)
            {
                try
                {
                    await _ingresoService.EliminarIngreso(_ingresoSeleccionado.Id);
                    Toast.MakeText(this.Activity, "Se ha eliminado el ingreso correctamente.", ToastLength.Long)
                        .Show();
                    LimpiarCampos();
                }
                catch
                {
                    Toast.MakeText(this.Activity, "Hubo un problema al tratar de eliminar el ingreso. Intente de nuevo mas tarde.",
                       ToastLength.Long).Show();
                }

                ActualizarIngresos();
            }
        }

        private async void _btnActualizar_Click(object sender, EventArgs e)
        {
            if (_ingresoSeleccionado != null)
            {
                try
                {
                    var ingreso = new Ingreso()
                    {
                        Id = _ingresoSeleccionado.Id,
                        Descripcion = _descripcion.Text,
                        Monto = Convert.ToDouble(_monto.Text),
                        Fecha = Convert.ToDateTime(_fechaIngreso.Text),
                        Fuente = _fuenteSeleccionada,
                        FuenteId = _fuenteSeleccionada.Id
                    };
                    await _ingresoService.ActualizarIngreso(_ingresoSeleccionado.Id, ingreso);
                    Toast.MakeText(this.Activity, "Se ha actualizado el servicio correctamente.", ToastLength.Long)
                        .Show();
                    LimpiarCampos();
                }
                catch
                {
                    Toast.MakeText(this.Activity, "Hubo un problema al tratar de actualizar el servicio. Intente de nuevo mas tarde.",
                       ToastLength.Long).Show();
                }

                ActualizarIngresos();
            }
        }

        private void _btnFechaIngreso_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _fechaIngreso.Text = time.ToLongDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void _fuente_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            _fuenteSeleccionada = ((ArrayAdapter<Fuente>)spinner.Adapter).GetItem(e.Position);
        }

        private void LimpiarCampos()
        {
            _listView.SetSelection(0);
            _monto.Text = string.Empty;
            _descripcion.Text = string.Empty;
            _fechaIngreso.Text = string.Empty;
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