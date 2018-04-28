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
using MyWalletApp.Mobile.Services;
using MyWalletApp.Mobile.Adapters;
using MyWalletApp.Mobile.Helpers;

namespace MyWalletApp.Mobile.Fragments
{
    public class ServiciosBuscarFragment : Fragment
    {
        private ListView _listView;
        private IEnumerable<Servicio> _servicios;
        private IList<Servicio> _filteredList;
        private Servicio _servicioSeleccionado;
        private Button _btnActualizar;
        private Button _btnEliminar;
        private Button _btnFechaPago;
        private EditText _buscar;
        private EditText _nombre;
        private EditText _monto;
        private Spinner _tipoPago;
        private TextView _fechaPago;
        private ProgressBar _loadingSpinner;
        private ServicioService servicioService;

        public ServiciosBuscarFragment()
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
            ActualizarServicios();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.ServiciosBuscarFragment, container, false);
        }

        private void FindViews()
        {
            _listView = View.FindViewById<ListView>(Resource.Id.serviciosListView);
            _btnActualizar = View.FindViewById<Button>(Resource.Id.btnActualizar);
            _btnEliminar = View.FindViewById<Button>(Resource.Id.btnEliminar);
            _btnFechaPago = View.FindViewById<Button>(Resource.Id.btnFechaPago);
            _buscar = View.FindViewById<EditText>(Resource.Id.buscar);
            _nombre = View.FindViewById<EditText>(Resource.Id.txtNombre);
            _monto = View.FindViewById<EditText>(Resource.Id.txtMonto);
            _tipoPago = View.FindViewById<Spinner>(Resource.Id.tipoPagoDropDown);
            _fechaPago = View.FindViewById<TextView>(Resource.Id.txtFechaPago);
            _loadingSpinner = View.FindViewById<ProgressBar>(Resource.Id.loadingSpinner);
        }

        private void HandleEvents()
        {
            _listView.ItemClick += _listView_ItemClick;
            _btnActualizar.Click += _btnActualizar_Click;
            _btnEliminar.Click += _btnEliminar_Click;
            _btnFechaPago.Click += _btnFechaPago_Click;
            _buscar.TextChanged += _buscar_TextChanged;
        }

        private void _buscar_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            var searchTerm = _buscar.Text;

            _filteredList = _servicios.Where(c => c.Nombre.Contains(searchTerm) || c.Monto.ToString().Contains(searchTerm)).ToList();

            var filteredAdapter = new ServicioListAdapter(this.Activity, _filteredList);
            _listView.Adapter = filteredAdapter;
        }

        private async void _btnEliminar_Click(object sender, EventArgs e)
        {
            if (_servicioSeleccionado != null)
            {
                try
                {
                    await servicioService.EliminarServicio(_servicioSeleccionado.Id);
                    Toast.MakeText(this.Activity, "Se ha eliminado el servicio correctamente.", ToastLength.Long)
                        .Show();
                    LimpiarCampos();
                }
                catch
                {
                    Toast.MakeText(this.Activity, "Hubo un problema al tratar de eliminar el servicio. Intente de nuevo mas tarde.",
                       ToastLength.Long).Show();
                }

                ActualizarServicios();
            }
        }

        private async void _btnActualizar_Click(object sender, EventArgs e)
        {
            if (_servicioSeleccionado != null)
            {
                try
                {
                    var servicio = new Servicio()
                    {
                        Id = _servicioSeleccionado.Id,
                        Nombre = _nombre.Text,
                        Monto = Convert.ToDouble(_monto.Text),
                        FechaPago = Convert.ToDateTime(_fechaPago.Text),
                        EsPorMes = _tipoPago.SelectedItem.ToString().ToLower().Equals("mensualmente") ? true : false
                    };
                    await servicioService.ActualizarServicio(_servicioSeleccionado.Id, servicio);
                    Toast.MakeText(this.Activity, "Se ha actualizado el servicio correctamente.", ToastLength.Long)
                        .Show();
                    LimpiarCampos();
                }
                catch
                {
                    Toast.MakeText(this.Activity, "Hubo un problema al tratar de actualizar el servicio. Intente de nuevo mas tarde.",
                       ToastLength.Long).Show();
                }

                ActualizarServicios();
            }
        }

        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            _servicioSeleccionado = _filteredList[e.Position];

            var index = Convert.ToBoolean(_servicioSeleccionado.EsPorMes) ? 0 : 1;

            _nombre.Text = _servicioSeleccionado.Nombre;
            _monto.Text = _servicioSeleccionado.Monto.ToString();
            _tipoPago.SetSelection(index);
            _fechaPago.Text = _servicioSeleccionado.FechaPago.ToString();
        }

        private void _btnFechaPago_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                _fechaPago.Text = time.ToLongDateString();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private async void ActualizarServicios()
        {
            try
            {
                _loadingSpinner.Visibility = ViewStates.Visible;

                _servicios = await servicioService.ObtenerServicios();
                _filteredList = _servicios.ToList();
                _servicioSeleccionado = null;
                _listView.Adapter = new ServicioListAdapter(this.Activity, _filteredList);

                ConfigurarAlturaListView();

                LimpiarCampos();
            }
            catch
            {
                //
            }
            finally
            {
                _loadingSpinner.Visibility = ViewStates.Gone;
            }
        }

        private void LimpiarCampos()
        {
            _listView.SetSelection(0);
            _nombre.Text = string.Empty;
            _monto.Text = string.Empty;
            _tipoPago.SetSelection(0);
            _fechaPago.Text = string.Empty;
        }

        public void LoadTipoPagos()
        {
            var array = TipoPago.TiposPago;

            var adapter = new ArrayAdapter<string>(this.Activity, Android.Resource.Layout.SimpleSpinnerItem, array);
            _tipoPago.Adapter = adapter;
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