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

namespace MyWalletApp.Mobile.Fragments.Fuentes
{
    public class FuentesBuscarFragment : Fragment
    {
        private ListView _listView;
        private IEnumerable<Fuente> _fuentes;
        private IList<Fuente> _filteredList;
        private Fuente _fuenteSeleccionada;
        private Button _btnActualizar;
        private Button _btnEliminar;
        private EditText _buscar;
        private EditText _nombre;
        private FuenteService _fuenteService;

        public FuentesBuscarFragment()
        {
            _fuenteService = new FuenteService();
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
            HandleEvents();
            ActualizarFuentes();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return inflater.Inflate(Resource.Layout.FuentesBuscarFragment, container, false);
        }

        private void FindViews()
        {
            _listView = View.FindViewById<ListView>(Resource.Id.fuentesListView);
            _btnActualizar = View.FindViewById<Button>(Resource.Id.btnActualizar);
            _btnEliminar = View.FindViewById<Button>(Resource.Id.btnEliminar);
            _buscar = View.FindViewById<EditText>(Resource.Id.buscar);
            _nombre = View.FindViewById<EditText>(Resource.Id.txtNombre);
        }

        private void HandleEvents()
        {
            _listView.ItemClick += _listView_ItemClick;
            _btnActualizar.Click += _btnActualizar_Click;
            _btnEliminar.Click += _btnEliminar_Click;
            _buscar.TextChanged += _buscar_TextChanged;
        }

        private void _buscar_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            var searchTerm = _buscar.Text;

            _filteredList = _fuentes.Where(c => c.Nombre.Contains(searchTerm)).ToList();

            var filteredAdapter = new FuenteListAdapter(this.Activity, _filteredList);
            _listView.Adapter = filteredAdapter;
        }

        private async void _btnEliminar_Click(object sender, EventArgs e)
        {
            if (_fuenteSeleccionada != null)
            {
                try
                {
                    await _fuenteService.EliminarFuente(_fuenteSeleccionada.Id);
                    Toast.MakeText(this.Activity, "Se ha eliminado la fuente correctamente.", ToastLength.Long)
                        .Show();
                    LimpiarCampos();
                }
                catch
                {
                    Toast.MakeText(this.Activity, "Hubo un problema al tratar de eliminar la fuente. Intente de nuevo mas tarde.",
                       ToastLength.Long).Show();
                }

                ActualizarFuentes();
            }
        }

        private async void _btnActualizar_Click(object sender, EventArgs e)
        {
            if (_fuenteSeleccionada != null)
            {
                try
                {
                    var fuente = new Fuente()
                    {
                        Id = _fuenteSeleccionada.Id,
                        Nombre = _nombre.Text
                    };
                    await _fuenteService.ActualizarFuente(_fuenteSeleccionada.Id, fuente);
                    Toast.MakeText(this.Activity, "Se ha actualizado la fuente correctamente.", ToastLength.Long)
                        .Show();
                    LimpiarCampos();
                }
                catch
                {
                    Toast.MakeText(this.Activity, "Hubo un problema al tratar de actualizar la fuente. Intente de nuevo mas tarde.",
                       ToastLength.Long).Show();
                }

                ActualizarFuentes();
            }
        }

        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            _fuenteSeleccionada = _filteredList[e.Position];
            _nombre.Text = _fuenteSeleccionada.Nombre;
        }

        private async void ActualizarFuentes()
        {
            _fuentes = await _fuenteService.ObtenerFuentes();
            _filteredList = _fuentes.ToList();
            _fuenteSeleccionada = null;
            _listView.Adapter = new FuenteListAdapter(this.Activity, _filteredList);

            ConfigurarAlturaListView();

            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            _listView.SetSelection(0);
            _nombre.Text = string.Empty;
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